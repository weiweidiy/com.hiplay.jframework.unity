using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class ModelRigisterGenerator : EditorWindow
{
    // 以Assets为根的相对路径
    private string sourceDir = "Assets/Downloads/HotfixScripts/Logic/Model";
    private string dtoDir = "Assets/Downloads/HotfixScripts/Protocol/DTOs";
    private string outputDir = "Assets/Downloads/HotfixScripts/AutoGen/Models";

    [MenuItem("JFrameworkTools/生成Model注册文件")]
    public static void ShowWindow()
    {
        Debug.Log("GameModelManagerGenerator EditorWindow 打开");
        var window = GetWindow<ModelRigisterGenerator>("GameModelManager生成器");
        window.minSize = new Vector2(300, 200);
    }

    void OnGUI()
    {
        GUILayout.Label("Model注册文件生成", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.Label("源目录:", GUILayout.Width(60));
        sourceDir = GUILayout.TextField(sourceDir, GUILayout.Width(300));
        if (GUILayout.Button("选择", GUILayout.Width(60)))
        {
            string path = EditorUtility.OpenFolderPanel("选择源目录", Application.dataPath, "");
            if (!string.IsNullOrEmpty(path))
            {
                if (path.Replace("\\", "/").StartsWith(Application.dataPath.Replace("\\", "/")))
                {
                    sourceDir = "Assets" + path.Substring(Application.dataPath.Length).Replace("\\", "/");
                }
                else
                {
                    sourceDir = path.Replace("\\", "/");
                }
            }
        }
        GUILayout.EndHorizontal();

        // 新增 DTO 目录选择
        GUILayout.BeginHorizontal();
        GUILayout.Label("DTO目录:", GUILayout.Width(60));
        dtoDir = GUILayout.TextField(dtoDir, GUILayout.Width(300));
        if (GUILayout.Button("选择", GUILayout.Width(60)))
        {
            string path = EditorUtility.OpenFolderPanel("选择DTO目录", Application.dataPath, "");
            if (!string.IsNullOrEmpty(path))
            {
                if (path.Replace("\\", "/").StartsWith(Application.dataPath.Replace("\\", "/")))
                {
                    dtoDir = "Assets" + path.Substring(Application.dataPath.Length).Replace("\\", "/");
                }
                else
                {
                    dtoDir = path.Replace("\\", "/");
                }
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("输出目录:", GUILayout.Width(60));
        outputDir = GUILayout.TextField(outputDir, GUILayout.Width(300));
        if (GUILayout.Button("选择", GUILayout.Width(60)))
        {
            string path = EditorUtility.OpenFolderPanel("选择输出目录", Application.dataPath, "");
            if (!string.IsNullOrEmpty(path))
            {
                if (path.Replace("\\", "/").StartsWith(Application.dataPath.Replace("\\", "/")))
                {
                    outputDir = "Assets" + path.Substring(Application.dataPath.Length).Replace("\\", "/");
                }
                else
                {
                    outputDir = path.Replace("\\", "/");
                }
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        if (GUILayout.Button("生成GameModelManager.cs", GUILayout.Height(40)))
        {
            if (string.IsNullOrEmpty(sourceDir) || string.IsNullOrEmpty(dtoDir) || string.IsNullOrEmpty(outputDir))
            {
                EditorUtility.DisplayDialog("错误", "请先选择源目录、DTO目录和输出目录", "确定");
                return;
            }
            Debug.Log($"源目录: {sourceDir}, DTO目录: {dtoDir}, 输出目录: {outputDir}");
            GenerateManagerFile();
        }
    }

    void GenerateManagerFile()
    {
        string absSourceDir = Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), sourceDir);
        string absDtoDir = Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), dtoDir);
        string absOutputDir = Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), outputDir);

        var modelInfos = new List<(string modelClass, string dtoType, string keyField)>();
        var modelFiles = Directory.GetFiles(absSourceDir, "*.cs", SearchOption.AllDirectories);
        var dtoFiles = Directory.GetFiles(absDtoDir, "*.cs", SearchOption.AllDirectories);
        var modelRegex = new Regex(@"public\s+class\s+(\w+)\s*:\s*Model<(\w+)>", RegexOptions.Compiled);

        foreach (var file in modelFiles)
        {
            var content = File.ReadAllText(file);
            var modelMatch = modelRegex.Match(content);
            if (modelMatch.Success)
            {
                string modelClass = modelMatch.Groups[1].Value;
                string dtoType = modelMatch.Groups[2].Value;
                string keyField = null;

                // 查找DTO定义文件
                string dtoFile = Array.Find(dtoFiles, f => Path.GetFileNameWithoutExtension(f) == dtoType);
                if (!string.IsNullOrEmpty(dtoFile) && File.Exists(dtoFile))
                {
                    var dtoContent = File.ReadAllText(dtoFile);
                    var keyRegex = new Regex(@"\[ModelKey\][\s\S]*?(?:public|private|protected)?\s*\w+\s+(\w+)\s*(?:\{[^\}]*\}|;)", RegexOptions.Compiled);
                    var keyMatch = keyRegex.Match(dtoContent);
                    if (keyMatch.Success)
                    {
                        keyField = keyMatch.Groups[1].Value;
                    }
                }

                if (string.IsNullOrEmpty(keyField))
                {
                    Debug.Log($"未找到 {dtoType} 中的 [ModelKey] 字段，默认使用 Uid 作为 key");
                    keyField = "Uid"; // 默认
                }

                modelInfos.Add((modelClass, dtoType, keyField));
            }
        }

        if (modelInfos.Count == 0)
        {
            EditorUtility.DisplayDialog("提示", "未找到任何 Model<> 派生类", "确定");
            return;
        }

        var code = @"using System;
using JFramework.Unity;
using System.Collections.Generic;

namespace Game
{
public class GameModelManager : BaseModelManager
{
    public override void RegisterModels()
    {
";
        foreach (var (modelClass, dtoType, keyField) in modelInfos)
        {
            code += $"        Func<{dtoType},string> func{modelClass} = (dto) => dto.{keyField};\n";
            code += $"        models.Add(nameof({modelClass}), new {modelClass}(func{modelClass}, Facade.GetEventManager() ));\n";
        }
        code += @"    }
}
}
";

        string outputPath = Path.Combine(absOutputDir, "GameModelManager.cs");
        File.WriteAllText(outputPath, code);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("完成", $"GameModelManager.cs 已生成到:\n{outputPath}", "确定");
    }
}
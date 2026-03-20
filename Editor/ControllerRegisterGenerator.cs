using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class ControllerRegisterGenerator : EditorWindow
{
    private string controllerSourceDir = "Assets/Downloads/HotfixScripts/Logic/Controller";
    private string controllerOutputDir = "Assets/Downloads/HotfixScripts/AutoGen/Controllers";
    private string controllerOutputFileName = "GameControllerManager";
    private Vector2 scrollPos;

    [MenuItem("JFrameworkTools/生成Controllers注册文件")]
    public static void ShowWindow()
    {
        var window = GetWindow<ControllerRegisterGenerator>("Controller注册生成器");
        window.minSize = new Vector2(500, 200);
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Label("Controller注册文件生成器", EditorStyles.boldLabel);
        controllerSourceDir = EditorGUILayout.TextField("源目录", controllerSourceDir);
        controllerOutputDir = EditorGUILayout.TextField("输出目录", controllerOutputDir);

        GUILayout.Space(10);
        if (GUILayout.Button("生成注册文件"))
        {
            GenerateRegisterFile(controllerSourceDir, controllerOutputDir, controllerOutputFileName);
        }

        EditorGUILayout.EndScrollView();
    }

    private void GenerateRegisterFile(string sourceDir, string outputDir, string viewOutputFileName)
    {
        if (!Directory.Exists(sourceDir))
        {
            EditorUtility.DisplayDialog("错误", "源目录不存在！", "确定");
            return;
        }
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        var csFiles = Directory.GetFiles(sourceDir, "*.cs", SearchOption.AllDirectories);
        var classNames = new HashSet<string>();

        // 正则匹配类名（支持 partial/public/internal/class/interface/struct）
        var classRegex = new Regex(@"\b(class|struct|interface)\s+([A-Za-z_][A-Za-z0-9_]*)", RegexOptions.Compiled);

        foreach (var file in csFiles)
        {
            var content = File.ReadAllText(file);
            foreach (Match match in classRegex.Matches(content))
            {
                var name = match.Groups[2].Value;
                classNames.Add(name);
            }
        }

        // 生成注册内容
        var tablesContent = "";
        foreach (var className in classNames)
        {
            //tablesContent += $"            tables.Add((int)ProtocolType.{className}, typeof({className}));\n";
            tablesContent += $"            controllers.Add(nameof({className}), new {className}());\n";
        }

        var fileContent = $@"using JFramework.Unity;


namespace Game
{{
    public class {viewOutputFileName} : BaseControllerManager
    {{
        public override void RegisterControllers()
        {{
            
{tablesContent}            
        }}
    }}
}}
";
        var filePath = Path.Combine(outputDir, viewOutputFileName + ".cs");
        File.WriteAllText(filePath, fileContent);

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("完成", $"已生成注册文件，包含 {classNames.Count} 个类型！", "确定");
    }
}
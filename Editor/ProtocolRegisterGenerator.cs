using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class ProtocolRegisterGenerator : EditorWindow
{
    private const string SourceDirPrefsKey = "ProtocolRegisterGenerator.SourceDir";
    private const string OutputDirPrefsKey = "ProtocolRegisterGenerator.OutputDir";

    private const string DefaultSourceDir = "Assets/Downloads/HotfixScripts/Protocol/SocketMessages";
    private const string DefaultOutputDir = "Assets/Downloads/HotfixScripts/AutoGen/Protocol";

    private string sourceDir = DefaultSourceDir;
    private string outputDir = DefaultOutputDir;
    private string outputFileName = "AutoNetMessageRegister.cs";
    private Vector2 scrollPos;

    [MenuItem("JFrameworkTools/生成Protocol注册文件")]
    public static void ShowWindow()
    {
        var window = GetWindow<ProtocolRegisterGenerator>("Protocol注册生成器");
        window.minSize = new Vector2(500, 200);
    }

    private void OnEnable()
    {
        sourceDir = EditorPrefs.GetString(SourceDirPrefsKey, DefaultSourceDir);
        outputDir = EditorPrefs.GetString(OutputDirPrefsKey, DefaultOutputDir);
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Label("Protocol注册文件生成器", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        sourceDir = EditorGUILayout.TextField("源目录", sourceDir);
        outputDir = EditorGUILayout.TextField("输出目录", outputDir);
        if (EditorGUI.EndChangeCheck())
        {
            SavePreferences();
        }

        GUILayout.Space(10);
        if (GUILayout.Button("生成注册文件"))
        {
            GenerateRegisterFile();
        }

        EditorGUILayout.EndScrollView();
    }

    private void SavePreferences()
    {
        EditorPrefs.SetString(SourceDirPrefsKey, sourceDir ?? string.Empty);
        EditorPrefs.SetString(OutputDirPrefsKey, outputDir ?? string.Empty);
    }

    private void GenerateRegisterFile()
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

        var tablesContent = "";
        foreach (var className in classNames)
        {
            tablesContent += $"            tables.Add((int)ProtocolType.{className}, typeof({className}));\n";
        }

        var fileContent = $@"using JFramework;
using System;
using System.Collections.Generic;

namespace Game
{{
    public class AutoNetMessageRegister : ITypeRegister
    {{
        public Dictionary<int, Type> GetTypes()
        {{
            var tables = new Dictionary<int, Type>();
{tablesContent}            return tables;
        }}
    }}
}}
";
        var filePath = Path.Combine(outputDir, outputFileName);
        File.WriteAllText(filePath, fileContent);

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("完成", $"已生成注册文件，包含 {classNames.Count} 个类型！", "确定");
    }
}
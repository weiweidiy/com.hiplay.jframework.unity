using UnityEditor;
using UnityEngine;
using System.IO;

public class CreateViewModuleTool : Editor
{
    [MenuItem("Assets/创建View模块", false, 100)]
    private static void CreateViewModule()
    {
        string path = GetSelectedPath();
        if (string.IsNullOrEmpty(path))
        {
            EditorUtility.DisplayDialog("错误", "请选择一个文件夹路径！", "确定");
            return;
        }

        CreateViewModuleWindow.ShowWindow(path);
    }

    private static string GetSelectedPath()
    {
        var obj = Selection.activeObject;
        if (obj == null)
            return null;
        string path = AssetDatabase.GetAssetPath(obj);
        if (Directory.Exists(path))
            return path;
        else
            return Path.GetDirectoryName(path);
    }
}

public class CreateViewModuleWindow : EditorWindow
{
    private string moduleName = "";
    private string targetPath;

    public static void ShowWindow(string path)
    {
        var window = ScriptableObject.CreateInstance<CreateViewModuleWindow>();
        window.titleContent = new GUIContent("创建View模块");
        window.targetPath = path;
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 350, 100);
        window.ShowUtility();
    }

    private void OnGUI()
    {
        GUILayout.Label("输入数据模块名（如 UIPanelLogin）", EditorStyles.boldLabel);
        moduleName = EditorGUILayout.TextField("模块名", moduleName);

        GUILayout.Space(10);
        if (GUILayout.Button("创建"))
        {
            if (string.IsNullOrWhiteSpace(moduleName))
            {
                EditorUtility.DisplayDialog("错误", "模块名不能为空！", "确定");
                return;
            }
            CreateFiles(moduleName, targetPath);
            Close();
        }
    }

    private void CreateFiles(string name, string path)
    {
        // xxx.cs
        string csFile = Path.Combine(path, $"{name}.cs");
        string csContent = $@"using System;
using UnityEngine;
using JFramework.Unity;
using deVoid.UIFramework;
namespace Game
{{

public class {name} : UIPanelBase<{name}Properties>
{{

    protected override void OnPanelHide()
    {{
        base.OnPanelHide();

    }}

    protected override void OnPanelShow()
    {{
        base.OnPanelShow();

    }}


}}

public class {name}Properties : PanelProperties
{{
    //public string prefabName;
}}
}}
";

        // xxxView.cs
        string viewFile = Path.Combine(path, $"{name}View.cs");
        string viewContent = $@"using System;
using JFramework.Unity;
namespace Game
{{
public class {name}View : View
{{
    {name} panel;
    public override void Close()
    {{
        throw new System.NotImplementedException();
    }}

    public override void Open<TArg>(TArg args)
    {{
        panel = GetUIManager().ShowPanel(args.prefabName, new {name}Properties()) as {name};
    }}

    public override void Refresh<TArg>(TArg args)
    {{
        throw new System.NotImplementedException();
    }}
}}
}}
";

        File.WriteAllText(csFile, csContent);
        File.WriteAllText(viewFile, viewContent);
        AssetDatabase.Refresh();
    }
}
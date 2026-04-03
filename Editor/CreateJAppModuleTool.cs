using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace JFramework.Unity.EditorTools
{
    public sealed class CreateJAppModuleTool : EditorWindow
    {
        private const string DefaultRootFolder = "Assets/Downloads/HotfixScripts";
        private const string WindowTitle = "Create IJApp Module";
        private const string ToolMenuPath = "JFrameworkTools/Create IJApp Module";
        private const string AssetsMenuPath = "Assets/JFramework/Create IJApp Module";

        private string moduleName = "MyModule";
        private string namespaceRoot = "Game";
        private string selectedFolder = DefaultRootFolder;
        private bool createEntry = true;
        private bool createFoundationModule = true;

        private bool createDefaultSceneState = true;
        private string defaultSceneStateName = "SceneLoginState";
        private string defaultSceneStateFolder = "SceneStates";

        [MenuItem(ToolMenuPath, false, 1)]
        private static void OpenWindow()
        {
            OpenWindowInternal(GetSelectedFolderOrDefault());
        }

        [MenuItem(AssetsMenuPath, false, 2000)]
        private static void OpenWindowFromAssetsMenu()
        {
            OpenWindowInternal(GetSelectedFolderOrDefault());
        }

        [MenuItem(AssetsMenuPath, true)]
        private static bool ValidateOpenWindowFromAssetsMenu()
        {
            return TryGetSelectedFolder(out _);
        }

        private static void OpenWindowInternal(string folder)
        {
            var window = GetWindow<CreateJAppModuleTool>(true, WindowTitle);
            window.minSize = new Vector2(520f, 260f);
            window.selectedFolder = folder;
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("生成 IJApp 模块模板", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            moduleName = EditorGUILayout.TextField("模块名", moduleName);
            namespaceRoot = EditorGUILayout.TextField("命名空间根", namespaceRoot);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.TextField("创建位置", selectedFolder);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("生成内容", EditorStyles.boldLabel);
            createEntry = EditorGUILayout.Toggle("创建入口脚本", createEntry);
            createFoundationModule = EditorGUILayout.Toggle("创建基础模块", createFoundationModule);
            createDefaultSceneState = EditorGUILayout.Toggle("创建默认SceneState", createDefaultSceneState);

            using (new EditorGUI.DisabledScope(!createDefaultSceneState))
            {
                defaultSceneStateName = EditorGUILayout.TextField("默认SceneState名", defaultSceneStateName);
                defaultSceneStateFolder = EditorGUILayout.TextField("SceneState目录", defaultSceneStateFolder);
            }

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(
                "将在当前选中的文件夹下，以模块名创建新目录，并自动生成 Modules、GameEntry、GameFoundationModule，以及 Model/View/Controller/Scene/Initializer 五个基础模块模板。可选生成首个 SceneState 文件到指定目录。",
                MessageType.Info);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("生成", GUILayout.Width(120f)))
                {
                    Generate();
                }
            }
        }

        private static string EnsureSceneStateSuffix(string name)
        {
            return name.EndsWith("State") ? name : name + "State";
        }

        private static string BuildSceneStateContent(string moduleNamespace, string sceneStateName)
        {
            var className = EnsureSceneStateSuffix(sceneStateName);

            return $@"using System;
using Cysharp.Threading.Tasks;
using JFramework.Unity;

namespace {moduleNamespace}
{{
    public sealed class {className} : BaseSceneState
    {{
        public override async UniTask EnterAsync(ISceneContext sceneContext, object arg)
        {{
            await base.EnterAsync(sceneContext, arg);
            throw new NotImplementedException();
        }}

        protected override string GetBGMClipName()
        {{
            throw new NotImplementedException();
        }}

        protected override string GetSceneName()
        {{
            throw new NotImplementedException();
        }}

        protected override string GetUISettingsName()
        {{
            throw new NotImplementedException();
        }}
    }}
}}
";
        }

        private void Generate()
        {
            var safeModuleName = NormalizeIdentifier(moduleName);
            if (string.IsNullOrWhiteSpace(safeModuleName))
            {
                EditorUtility.DisplayDialog("无效名称", "请输入合法的 C# 模块名。", "确定");
                return;
            }

            var safeNamespaceRoot = NormalizeNamespace(namespaceRoot);
            if (string.IsNullOrWhiteSpace(safeNamespaceRoot))
            {
                EditorUtility.DisplayDialog("无效命名空间", "命名空间根不能为空，且必须是合法的 C# 命名空间。", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(selectedFolder) || !selectedFolder.StartsWith("Assets"))
            {
                EditorUtility.DisplayDialog("无效目录", "请先在 Project 视图中选择一个有效文件夹。", "确定");
                return;
            }

            var normalizedSceneStateFolder = NormalizeAssetRelativeFolder(defaultSceneStateFolder);
            var safeDefaultSceneStateTypeName = EnsureSceneStateSuffix(defaultSceneStateName);

            if (createDefaultSceneState)
            {
                var safeSceneStateName = NormalizeIdentifier(safeDefaultSceneStateTypeName);
                if (string.IsNullOrWhiteSpace(safeSceneStateName))
                {
                    EditorUtility.DisplayDialog("无效名称", "默认SceneState名不能为空，且必须是合法的 C# 类型名。", "确定");
                    return;
                }

                if (string.IsNullOrWhiteSpace(normalizedSceneStateFolder))
                {
                    EditorUtility.DisplayDialog("无效目录", "SceneState目录不能为空。", "确定");
                    return;
                }

                safeDefaultSceneStateTypeName = safeSceneStateName;
            }
            else
            {
                safeDefaultSceneStateTypeName = "SceneLoginState";
            }

            var moduleRootFolder = CombineAssetPath(selectedFolder, safeModuleName);
            var modulesFolder = CombineAssetPath(moduleRootFolder, "Modules");
            var sceneStateFolder = createDefaultSceneState
                ? CombineAssetPath(moduleRootFolder, normalizedSceneStateFolder)
                : string.Empty;

            var controllersFolder = CombineAssetPath(moduleRootFolder, "Controllers");
            var modelsFolder = CombineAssetPath(moduleRootFolder, "Models");
            var viewsFolder = CombineAssetPath(moduleRootFolder, "Views");
            var dtosFolder = CombineAssetPath(moduleRootFolder, "DTOs");

            if (AssetDatabase.IsValidFolder(moduleRootFolder) || Directory.Exists(ToAbsolutePath(moduleRootFolder)))
            {
                EditorUtility.DisplayDialog("目录已存在", $"目标目录已存在：\n{moduleRootFolder}", "确定");
                return;
            }

            Directory.CreateDirectory(ToAbsolutePath(modulesFolder));
            Directory.CreateDirectory(ToAbsolutePath(controllersFolder));
            Directory.CreateDirectory(ToAbsolutePath(modelsFolder));
            Directory.CreateDirectory(ToAbsolutePath(viewsFolder));
            Directory.CreateDirectory(ToAbsolutePath(dtosFolder));

            if (createDefaultSceneState)
            {
                Directory.CreateDirectory(ToAbsolutePath(sceneStateFolder));
            }

            var moduleNamespace = $"{safeNamespaceRoot}.{safeModuleName}";

            try
            {
                if (createEntry)
                {
                    WriteFileIfNotExists(modulesFolder, "GameEntry.cs", BuildEntryContent(moduleNamespace));
                }

                if (createFoundationModule)
                {
                    WriteFileIfNotExists(modulesFolder, "GameFoundationModule.cs", BuildFoundationModuleContent(moduleNamespace));
                }

                WriteFileIfNotExists(modulesFolder, "GameModules.cs", BuildGameModulesContent(moduleNamespace));
                WriteFileIfNotExists(modulesFolder, "GameOtherRegistryModule.cs", BuildGameOtherRegistryModuleContent(moduleNamespace));
                WriteFileIfNotExists(modulesFolder, "ModelRegistryModule.cs", BuildModelRegistryModuleContent(moduleNamespace));
                WriteFileIfNotExists(modulesFolder, "ViewRegistryModule.cs", BuildViewRegistryModuleContent(moduleNamespace));
                WriteFileIfNotExists(modulesFolder, "ControllerRegistryModule.cs", BuildControllerRegistryModuleContent(moduleNamespace));
                WriteFileIfNotExists(modulesFolder, "SceneRegistryModule.cs", BuildSceneRegistryModuleContent(moduleNamespace, safeDefaultSceneStateTypeName));
                WriteFileIfNotExists(modulesFolder, "InitializerModule.cs", BuildInitializerModuleContent(moduleNamespace));

                if (createDefaultSceneState)
                {
                    var sceneStateFileName = safeDefaultSceneStateTypeName + ".cs";
                    WriteFileIfNotExists(
                        sceneStateFolder,
                        sceneStateFileName,
                        BuildSceneStateContent(moduleNamespace, safeDefaultSceneStateTypeName));
                }

                AssetDatabase.Refresh();

                var folderAsset = AssetDatabase.LoadAssetAtPath<Object>(moduleRootFolder);
                Selection.activeObject = folderAsset;
                EditorGUIUtility.PingObject(folderAsset);

                EditorUtility.DisplayDialog("完成", $"模板已生成到：\n{moduleRootFolder}", "确定");
                Close();
            }
            catch (IOException exception)
            {
                EditorUtility.DisplayDialog("生成失败", exception.Message, "确定");
            }
        }

        private static string BuildGameModulesContent(string moduleNamespace)
        {
            return $@"using JFramework.Unity;

namespace {moduleNamespace}
{{
    public sealed class GameModules : IModuleInstaller
    {{
        private static readonly IModuleInstaller[] Modules =
        {{
            new ModelRegistryModule(),
            new ViewRegistryModule(),
            new ControllerRegistryModule(),
            new SceneRegistryModule(),
            new InitializerModule(),
            new GameOtherRegistryModule(),
        }};

        public void Install(IServiceRegistry services)
        {{
            foreach (var module in Modules)
            {{
                module.Install(services);
            }}
        }}
    }}
}}
";
        }

        private static string BuildGameOtherRegistryModuleContent(string moduleNamespace)
        {
            return $@"using JFramework.Unity;

namespace {moduleNamespace}
{{
    /// <summary>
    /// 所有游戏查询相关的功能都可以放在这个模块中，提供给其他模块使用。
    /// </summary>
    public class GameOtherRegistryModule : IModuleInstaller
    {{
        public void Install(IServiceRegistry services)
        {{
            
        }}
    }}
}}
";
        }

        private static string BuildEntryContent(string moduleNamespace)
        {
            return $@"using JFramework.Unity;
using UnityEngine;

namespace {moduleNamespace}
{{
    [DisallowMultipleComponent]
    public sealed class GameEntry : MonoBehaviour
    {{
        private IJApp app;

        private async void Start()
        {{
            app = new JAppBuilder()
                .AddModule(new GameFoundationModule())
                .AddModule(new GameModules())
                .Build();

            await app.RunAsync();
        }}

        private async void OnDestroy()
        {{
            if (app != null)
            {{
                await app.ShutdownAsync();
            }}
        }}
    }}
}}
";
        }

        private static string BuildFoundationModuleContent(string moduleNamespace)
        {
            return $@"using JFramework;
using JFramework.Game;
using JFramework.Unity;

namespace {moduleNamespace}
{{
    public class GameFoundationModule : DefaultFoundationModule
    {{
        public override void Install(IServiceRegistry services)
        {{
            base.Install(services);

            // 这里用于安装游戏基础设施相关服务。
            // 可在此注册日志、网络、配置、资源加载、性能监控等通用能力。
            // 建议仅放置跨业务模块共享的基础服务。
        }}
    }}
}}
";
        }



        private static string BuildModelRegistryModuleContent(string moduleNamespace)
        {
            return $@"using JFramework.Unity;
using JFramework;

namespace {moduleNamespace}
{{
    public sealed class ModelRegistryModule : IModuleInstaller
    {{
        public void Install(IServiceRegistry services)
        {{
            if (!services.TryResolve<IModelRegistry>(out _))
            {{
                services.AddSingleton<IModelRegistry>(new ModelRegistry());
            }}

            var eventManager = services.Resolve<EventManager>();
            var models = services.Resolve<IModelRegistry>();

            // <auto-generated-model-registrations>
            // </auto-generated-model-registrations>
        }}
    }}
}}
";
        }

        private static string BuildViewRegistryModuleContent(string moduleNamespace)
        {
            return $@"using JFramework.Unity;

namespace {moduleNamespace}
{{
    public sealed class ViewRegistryModule : IModuleInstaller
    {{
        public void Install(IServiceRegistry services)
        {{
            if (!services.TryResolve<IViewRegistry>(out _))
            {{
                services.AddSingleton<IViewRegistry>(new ViewRegistry());
            }}

            var views = services.Resolve<IViewRegistry>();

            // <auto-generated-view-registrations>
            // </auto-generated-view-registrations>
        }}
    }}
}}
";
        }

        private static string BuildControllerRegistryModuleContent(string moduleNamespace)
        {
            return $@"using JFramework.Unity;

namespace {moduleNamespace}
{{
    public sealed class ControllerRegistryModule : IModuleInstaller
    {{
        public void Install(IServiceRegistry services)
        {{
            if (!services.TryResolve<IControllerRegistry>(out _))
            {{
                services.AddSingleton<IControllerRegistry>(new ControllerRegistry());
            }}

            var controllers = services.Resolve<IControllerRegistry>();

            // <auto-generated-controller-registrations>
            // </auto-generated-controller-registrations>
        }}
    }}
}}
";
        }

        private static string BuildSceneRegistryModuleContent(string moduleNamespace, string defaultSceneStateTypeName)
        {
            return $@"using JFramework.Unity;

namespace {moduleNamespace}
{{
    public sealed class SceneRegistryModule : IModuleInstaller
    {{
        public void Install(IServiceRegistry services)
        {{
            if (!services.TryResolve<ISceneStateRegistry>(out _))
                services.AddSingleton<ISceneStateRegistry>(new SceneStateRegistry());

            var sceneStates = services.Resolve<ISceneStateRegistry>();

            // <auto-generated-scene-state-registrations>
            if (!sceneStates.TryGet<{defaultSceneStateTypeName}>(out _))
                sceneStates.Register(new {defaultSceneStateTypeName}());
            // </auto-generated-scene-state-registrations>

            if (!services.TryResolve<GameContext>(out var context))
            {{
                context = new GameContext();
                services.AddSingleton(context);
            }}

            if (!services.TryResolve<ISceneContext>(out _))
            {{
                var sceneContext = new SceneContext(services);
                services.AddSingleton<ISceneContext>(sceneContext);
                services.AddSingleton<IViewContext>(sceneContext);
            }}

            if (!services.TryResolve<ISceneFlow>(out _))
            {{
                services.AddSingleton<ISceneFlow>(
                    new SceneFlowService(
                        services.Resolve<ISceneStateRegistry>(),
                        services.Resolve<ISceneContext>(),
                        typeof({defaultSceneStateTypeName})));
            }}
        }}
    }}
}}
";
        }

        private static string BuildInitializerModuleContent(string moduleNamespace)
        {
            return $@"using JFramework.Unity;

namespace {moduleNamespace}
{{
    public sealed class InitializerModule : IModuleInstaller
    {{
        public void Install(IServiceRegistry services)
        {{
            // 在这里执行模块初始化逻辑。
            // 例如：
            // 1. 初始化配置表
            // 2. 初始化网络连接
            // 3. 进入默认场景状态
        }}
    }}
}}
";
        }

        private static void WriteFileIfNotExists(string assetFolder, string fileName, string content)
        {
            var filePath = Path.Combine(ToAbsolutePath(assetFolder), fileName);
            if (File.Exists(filePath))
                throw new IOException($"文件已存在：{assetFolder}/{fileName}");

            File.WriteAllText(filePath, content, new UTF8Encoding(false));
        }

        private static string NormalizeIdentifier(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var trimmed = Regex.Replace(value.Trim(), @"\s+", string.Empty);
            return Regex.IsMatch(trimmed, @"^[A-Za-z_][A-Za-z0-9_]*$") ? trimmed : string.Empty;
        }

        private static string NormalizeNamespace(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var trimmed = value.Trim();
            if (!Regex.IsMatch(trimmed, @"^[A-Za-z_][A-Za-z0-9_]*(\.[A-Za-z_][A-Za-z0-9_]*)*$"))
                return string.Empty;

            return trimmed;
        }

        private static string NormalizeAssetRelativeFolder(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
                return string.Empty;

            return folder.Trim().Trim('/').Trim('\\').Replace("\\", "/");
        }

        private static string GetSelectedFolderOrDefault()
        {
            return TryGetSelectedFolder(out var folder) ? folder : DefaultRootFolder;
        }

        private static bool TryGetSelectedFolder(out string folder)
        {
            folder = null;

            var selected = Selection.activeObject;
            if (selected == null)
                return false;

            var path = AssetDatabase.GetAssetPath(selected);
            if (string.IsNullOrEmpty(path))
                return false;

            if (!AssetDatabase.IsValidFolder(path))
                return false;

            folder = path;
            return true;
        }

        private static string CombineAssetPath(string left, string right)
        {
            return $"{left.TrimEnd('/')}/{right}";
        }

        private static string ToAbsolutePath(string assetPath)
        {
            var projectRoot = Directory.GetCurrentDirectory();
            return Path.Combine(projectRoot, assetPath.Replace("/", Path.DirectorySeparatorChar.ToString()));
        }
    }
}
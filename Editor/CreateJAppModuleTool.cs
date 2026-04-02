using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace JFramework.Unity.EditorTools
{
    public sealed class CreateJAppModuleTool : EditorWindow
    {
        private const string DefaultRootFolder = "Assets/Scenes";
        private const string WindowTitle = "Create IJApp Module";

        private string moduleName = "NewModule";
        private string namespaceRoot = "Game";
        private string targetFolder = DefaultRootFolder;
        private bool createEntry = true;

        [MenuItem("Tools/JFramework/Create IJApp Module", false, 1)]
        private static void OpenWindow()
        {
            var window = GetWindow<CreateJAppModuleTool>(true, WindowTitle);
            window.minSize = new Vector2(460f, 220f);
            window.targetFolder = GetSelectedFolderOrDefault();
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Generate a complete IJApp module scaffold.", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            moduleName = EditorGUILayout.TextField("Module Name", moduleName);
            namespaceRoot = EditorGUILayout.TextField("Namespace Root", namespaceRoot);
            targetFolder = EditorGUILayout.TextField("Target Folder", targetFolder);
            createEntry = EditorGUILayout.Toggle("Create Entry", createEntry);

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(
                "This will create GameModule, RegistryModule, PresentationModule, RuntimeModule, a sample State/View/Controller/Model, and README.",
                MessageType.Info);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Generate", GUILayout.Width(120f)))
                {
                    Generate();
                }
            }
        }

        private void Generate()
        {
            var safeModuleName = NormalizeIdentifier(moduleName);
            if (string.IsNullOrWhiteSpace(safeModuleName))
            {
                EditorUtility.DisplayDialog("Invalid Name", "Please enter a valid C# identifier for the module name.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(namespaceRoot))
            {
                EditorUtility.DisplayDialog("Invalid Namespace", "Namespace root cannot be empty.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(targetFolder) || !targetFolder.StartsWith("Assets"))
            {
                EditorUtility.DisplayDialog("Invalid Folder", "Target folder must be a valid Unity Assets path.", "OK");
                return;
            }

            var moduleFolder = CombineAssetPath(targetFolder, safeModuleName);
            if (AssetDatabase.IsValidFolder(moduleFolder) || File.Exists(moduleFolder))
            {
                EditorUtility.DisplayDialog("Folder Exists", $"Target folder already exists: {moduleFolder}", "OK");
                return;
            }

            Directory.CreateDirectory(ToAbsolutePath(moduleFolder));

            var moduleNamespace = $"{namespaceRoot}.{safeModuleName}";
            var homeStateName = $"{safeModuleName}HomeState";
            var modelName = $"{safeModuleName}StatusModel";
            var controllerName = $"{safeModuleName}StartController";
            var viewName = $"{safeModuleName}StatusView";

            WriteFile(moduleFolder, $"{safeModuleName}GameModule.cs", BuildGameModuleContent(safeModuleName, moduleNamespace));
            WriteFile(moduleFolder, $"{safeModuleName}RegistryModule.cs", BuildRegistryModuleContent(safeModuleName, moduleNamespace, homeStateName, modelName, controllerName, viewName));
            WriteFile(moduleFolder, $"{safeModuleName}PresentationModule.cs", BuildPresentationModuleContent(safeModuleName, moduleNamespace));
            WriteFile(moduleFolder, $"{safeModuleName}RuntimeModule.cs", BuildRuntimeModuleContent(safeModuleName, moduleNamespace, homeStateName));
            WriteFile(moduleFolder, $"{homeStateName}.cs", BuildHomeStateContent(moduleNamespace, homeStateName, modelName, controllerName, viewName));
            WriteFile(moduleFolder, $"{modelName}.cs", BuildModelContent(moduleNamespace, modelName));
            WriteFile(moduleFolder, $"{controllerName}.cs", BuildControllerContent(moduleNamespace, controllerName, modelName));
            WriteFile(moduleFolder, $"{viewName}.cs", BuildViewContent(moduleNamespace, viewName, modelName));
            WriteFile(moduleFolder, "README.md", BuildReadmeContent(safeModuleName, homeStateName));

            if (createEntry)
            {
                WriteFile(moduleFolder, $"{safeModuleName}Entry.cs", BuildEntryContent(safeModuleName, moduleNamespace));
            }

            AssetDatabase.Refresh();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(moduleFolder);
            EditorGUIUtility.PingObject(Selection.activeObject);
            Close();
        }

        private static string BuildGameModuleContent(string moduleName, string moduleNamespace)
        {
            return $@"using JFramework.Unity;

namespace {moduleNamespace}
{{
    public sealed class {moduleName}GameModule : IModuleInstaller
    {{
        private static readonly IModuleInstaller[] Modules =
        {{
            new {moduleName}RegistryModule(),
            new {moduleName}PresentationModule(),
            new {moduleName}RuntimeModule(),
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

        private static string BuildRegistryModuleContent(string moduleName, string moduleNamespace, string stateName, string modelName, string controllerName, string viewName)
        {
            return $@"using JFramework.Unity;

namespace {moduleNamespace}
{{
    public sealed class {moduleName}RegistryModule : IModuleInstaller
    {{
        public void Install(IServiceRegistry services)
        {{
            if (!services.TryResolve<IModelRegistry>(out _))
                services.AddSingleton<IModelRegistry>(new ModelRegistry());

            if (!services.TryResolve<IControllerRegistry>(out _))
                services.AddSingleton<IControllerRegistry>(new ControllerRegistry());

            if (!services.TryResolve<IViewRegistry>(out _))
                services.AddSingleton<IViewRegistry>(new ViewRegistry());

            if (!services.TryResolve<ISceneStateRegistry>(out _))
                services.AddSingleton<ISceneStateRegistry>(new SceneStateRegistry());

            var models = services.Resolve<IModelRegistry>();
            if (!models.TryGet<{modelName}>(out _))
                models.Register(new {modelName}());

            var controllers = services.Resolve<IControllerRegistry>();
            if (!controllers.TryGet<{controllerName}>(out _))
                controllers.Register(new {controllerName}());

            var views = services.Resolve<IViewRegistry>();
            views.RegisterForScene(typeof({stateName}), new {viewName}());

            var states = services.Resolve<ISceneStateRegistry>();
            if (!states.TryGet<{stateName}>(out _))
                states.Register(new {stateName}());
        }}
    }}
}}
";
        }

        private static string BuildPresentationModuleContent(string moduleName, string moduleNamespace)
        {
            return $@"using JFramework.Unity;

namespace {moduleNamespace}
{{
    public sealed class {moduleName}PresentationModule : IModuleInstaller
    {{
        public void Install(IServiceRegistry services)
        {{
            // Keep presentation-specific registrations here when the module grows.
        }}
    }}
}}
";
        }

        private static string BuildRuntimeModuleContent(string moduleName, string moduleNamespace, string stateName)
        {
            return $@"using JFramework.Unity;

namespace {moduleNamespace}
{{
    public sealed class {moduleName}RuntimeModule : IModuleInstaller
    {{
        public void Install(IServiceRegistry services)
        {{
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
                        typeof({stateName})));
            }}
        }}
    }}
}}
";
        }

        private static string BuildHomeStateContent(string moduleNamespace, string stateName, string modelName, string controllerName, string viewName)
        {
            return $@"using System;
using Cysharp.Threading.Tasks;
using JFramework.Unity;

namespace {moduleNamespace}
{{
    public sealed class {stateName} : ISceneState
    {{
        public string Name => nameof({stateName});

        public async UniTask EnterAsync(ISceneContext context, object arg)
        {{
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var gameContext = new GameContext
            {{
                Services = context.Services
            }};

            var controller = context.Controllers.Get<{controllerName}>();
            await controller.Do(gameContext);

            var model = context.Models.Get<{modelName}>();
            var views = context.Views.GetViewsForScene(typeof({stateName}));
            foreach (var view in views)
            {{
                view.Start(gameContext);
                view.Open(new ViewData {{ prefabName = model.ViewName }});
            }}
        }}

        public UniTask ExitAsync()
        {{
            return UniTask.CompletedTask;
        }}
    }}
}}
";
        }

        private static string BuildModelContent(string moduleNamespace, string modelName)
        {
            return $@"namespace {moduleNamespace}
{{
    public sealed class {modelName}
    {{
        public string StageName {{ get; set; }} = ""{modelName.Replace("StatusModel", "Stage")}"";

        public string ViewName {{ get; set; }} = ""{modelName.Replace("StatusModel", "StatusView")}"";

        public int RecommendedPower {{ get; set; }} = 1000;

        public bool Ready {{ get; set; }}
    }}
}}
";
        }

        private static string BuildControllerContent(string moduleNamespace, string controllerName, string modelName)
        {
            return $@"using System.Threading.Tasks;
using JFramework.Unity;
using UnityEngine;

namespace {moduleNamespace}
{{
    public sealed class {controllerName} : Controller
    {{
        public override Task Do(GameContext context, params object[] parameters)
        {{
            var services = context?.Services
                ?? throw new System.InvalidOperationException(""GameContext.Services is required."");

            var model = services.Resolve<IModelRegistry>().Get<{modelName}>();
            model.Ready = true;

            Debug.Log($""[{moduleNamespace}] Ready for stage {{model.StageName}}, recommended power {{model.RecommendedPower}}."");
            return Task.CompletedTask;
        }}
    }}
}}
";
        }

        private static string BuildViewContent(string moduleNamespace, string viewName, string modelName)
        {
            return $@"using JFramework.Unity;
using UnityEngine;

namespace {moduleNamespace}
{{
    public sealed class {viewName} : View
    {{
        public override void Open<TArg>(TArg args)
        {{
            var model = context.Services.Resolve<IModelRegistry>().Get<{modelName}>();
            Debug.Log($""[{viewName}] Open stage={{model.StageName}}, power={{model.RecommendedPower}}, ready={{model.Ready}}"");
        }}

        public override void Close()
        {{
            Debug.Log(""{viewName} Close"");
        }}

        public override void Refresh<TArg>(TArg args)
        {{
            var model = context.Services.Resolve<IModelRegistry>().Get<{modelName}>();
            Debug.Log($""[{viewName}] Refresh stage={{model.StageName}}, ready={{model.Ready}}"");
        }}
    }}
}}
";
        }

        private static string BuildEntryContent(string moduleName, string moduleNamespace)
        {
            return $@"using JFramework.Unity;
using UnityEngine;

namespace {moduleNamespace}
{{
    [DisallowMultipleComponent]
    public sealed class {moduleName}Entry : MonoBehaviour
    {{
        private IJApp app;

        private async void Start()
        {{
            app = new JAppBuilder()
                .AddModule(new DefaultFoundationModule())
                .AddModule(new {moduleName}GameModule())
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

        private static string BuildReadmeContent(string moduleName, string stateName)
        {
            return $@"# {moduleName}

This module scaffold was generated for the IJApp workflow.

Generated files:

- `{moduleName}GameModule.cs`
- `{moduleName}RegistryModule.cs`
- `{moduleName}PresentationModule.cs`
- `{moduleName}RuntimeModule.cs`
- `{stateName}.cs`
- `{moduleName}StatusModel.cs`
- `{moduleName}StartController.cs`
- `{moduleName}StatusView.cs`
- `{moduleName}Entry.cs` (optional)

Recommended next steps:

1. Replace sample logs with real business logic.
2. Add more states and register them in `{moduleName}RegistryModule`.
3. Split services into the module when the feature grows.
4. Use this folder as the single source of truth for the module.
";
        }

        private static void WriteFile(string assetFolder, string fileName, string content)
        {
            var filePath = Path.Combine(ToAbsolutePath(assetFolder), fileName);
            File.WriteAllText(filePath, content, new UTF8Encoding(false));
        }

        private static string NormalizeIdentifier(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var trimmed = Regex.Replace(value.Trim(), @"\s+", string.Empty);
            return Regex.IsMatch(trimmed, @"^[A-Za-z_][A-Za-z0-9_]*$") ? trimmed : string.Empty;
        }

        private static string GetSelectedFolderOrDefault()
        {
            var selected = Selection.activeObject;
            if (selected == null)
                return DefaultRootFolder;

            var path = AssetDatabase.GetAssetPath(selected);
            if (string.IsNullOrEmpty(path))
                return DefaultRootFolder;

            if (AssetDatabase.IsValidFolder(path))
                return path;

            var directory = Path.GetDirectoryName(path);
            return string.IsNullOrEmpty(directory) ? DefaultRootFolder : directory.Replace("\\", "/");
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

# JFrameworkUnity Quick Start

## 1. Recommended Entry

Use `IJApp` as the only startup mode.

Minimal entry example:

```csharp
using JFramework.Unity;
using UnityEngine;

public class GameEntry : MonoBehaviour
{
    private IJApp app;

    private async void Start()
    {
        app = new JAppBuilder()
            .AddModule(new DefaultFoundationModule())
            .AddModule(new YourGameModule())
            .Build();

        await app.RunAsync();
    }

    private async void OnDestroy()
    {
        if (app != null)
            await app.ShutdownAsync();
    }
}
```

## 2. Minimal Setup Steps

1. Add one entry component to the scene, such as `GameEntry`.
2. Install `DefaultFoundationModule` with `JAppBuilder`.
3. Install your own game module.
4. Register services, states, views, controllers, and models in that module.
5. Let `ISceneFlow` enter the first state.

## 3. Create a Module

Fastest option:

- Open `Tools/JFramework/Create IJApp Module`
- Enter a module name such as `Inventory` or `Battle`
- Generate the scaffold directly under your selected folder

Manual option:

Implement `IModuleInstaller`:

```csharp
public sealed class YourGameModule : IModuleInstaller
{
    public void Install(IServiceRegistry services)
    {
        // register services
        // register registries
        // register states
        // register models / controllers / views
    }
}
```

Recommended split:

- `RegistryModule`
- `PresentationModule`
- `RuntimeModule`
- `GameModule`

## 4. Register Dependencies

Register a service:

```csharp
services.AddSingleton<IInventoryService>(new InventoryService());
```

Resolve a service:

```csharp
var inventory = context.Services.Resolve<IInventoryService>();
```

Guideline:

- Put infrastructure into `IServiceRegistry`.
- Put business objects into typed registries.

## 5. Register Business Objects

Register a model:

```csharp
var models = services.Resolve<IModelRegistry>();
models.Register(new PlayerModel());
```

Register a controller:

```csharp
var controllers = services.Resolve<IControllerRegistry>();
controllers.Register(new LoginController());
```

Register a view:

```csharp
var views = services.Resolve<IViewRegistry>();
views.RegisterForScene(typeof(LoginSceneState), new LoginView());
```

## 6. Write States

Recommended base classes:

- `BaseStateAsync`
- `BaseSceneState<TSceneType>`

Resolve dependencies like this:

```csharp
var services = context.Services;
var ui = services.Resolve<IJUIManager>();
```

Do not add new `Facade` dependencies to states.

## 7. Write Views

Continue inheriting from `View`.

Recommended approach:

- Use base helpers to access `UI`, `EventManager`, and `GameObjectManager`.
- Use `context.Services` or registries for business dependencies.
- Do not depend on any global facade-style entry object.

Example:

```csharp
panel = GetUIManager().ShowPanel(...);
```

## 8. Write Controllers

Continue inheriting from the current `Controller` base type:

```csharp
public abstract class Controller
{
    public abstract Task Do(GameContext context, params object[] parameters);
}
```

Recommended approach:

- Resolve dependencies from `context.Services`.
- Treat controllers as action executors.
- Do not assume `Facade` exists.

## 9. Dependency Order

Resolve dependencies in this order:

1. `context.Services`
2. `IModelRegistry / IControllerRegistry / IViewRegistry`
3. Small context abstractions

Do not add new `Facade` fallback logic.

## 10. Avoid These Patterns

Do not:

- use any facade-style startup entry
- use a global dependency hub for new code
- add new string-based lookup APIs
- expand the responsibility of old managers

## 11. Reference Files

- Unified entry: `Assets/Scenes/Demo.cs`
- New module example: `Assets/Scenes/Battle/README.md`
- Architecture guide: `Assets/com.hiplay.jframework.unity/Runtime/Scripts/ARCHITECTURE_MIGRATION.md`

Use this file when you want the shortest path to start building on the current architecture.

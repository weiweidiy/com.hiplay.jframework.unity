# JFrameworkUnity IJApp 架构说明与使用指南

## 1. 当前结论

当前项目只保留 `IJApp` 启动模式。

唯一推荐入口是：

- `Assets/Scenes/Demo.cs`
- `JAppBuilder`
- `DefaultFoundationModule`
- `DemoGameModule`

旧的 `FacadeBuilder + JFacade` 启动方式已经从项目中移除。
当前运行时主路径只包含 `IJApp + JAppBuilder + Module + ServiceRegistry + Registry + SceneFlow`。

---

## 2. 整体架构

当前重构后的结构可以理解为 4 层：

- `Foundation`
  - 提供基础设施能力
  - 例如：资源、事件、HTTP、对象池、配置、音频、存档
- `Runtime`
  - 负责应用装配、生命周期、依赖注册、模块安装
- `Presentation`
  - 负责 View、UI、场景流转、过场抽象
- `Game`
  - 负责具体业务逻辑，例如 Demo 的状态、Controller、View、Model

推荐的理解方式是：

- `Foundation` 解决“能不能提供基础能力”
- `Runtime` 解决“怎么把这些能力组装成应用”
- `Presentation` 解决“怎么驱动界面和场景表现”
- `Game` 解决“具体业务要做什么”

---

## 3. 现在的启动方式

### 3.1 统一入口

当前统一入口文件：

- `Assets/Scenes/Demo.cs`

当前写法：

```csharp
app = new JAppBuilder()
    .AddModule(new DefaultFoundationModule())
    .AddModule(new DemoGameModule())
    .Build();

await app.RunAsync();
```

退出时：

```csharp
if (app != null)
    await app.ShutdownAsync();
```

这就是当前推荐的标准启动模板。

### 3.2 启动链路

`Demo.cs` 启动后，链路如下：

1. `JAppBuilder` 创建应用
2. 安装 `DefaultFoundationModule`
3. 安装 `DemoGameModule`
4. 构建 `JApp`
5. `RunAsync()` 执行初始化器
6. 通过 `ISceneFlow` 进入首个状态

---

## 4. 核心概念

### 4.1 IJApp

`IJApp` 是新的应用入口抽象。

职责只有三件事：

- 获取服务 `GetService<T>()`
- 启动应用 `RunAsync()`
- 关闭应用 `ShutdownAsync()`

建议把它理解成“应用容器”，而不是“全能大 Facade”。

### 4.2 JAppBuilder

`JAppBuilder` 用来装配应用。

它负责：

- 创建服务容器
- 安装模块
- 收集初始化器与释放对象
- 构建 `IJApp`

推荐用法：

```csharp
var app = new JAppBuilder()
    .AddModule(new DefaultFoundationModule())
    .AddModule(new YourGameModule())
    .Build();
```

### 4.3 IServiceRegistry

`IServiceRegistry` 是服务注册中心。

它负责：

- 注册服务
- 解析服务
- 给模块、状态、View、Controller 提供统一依赖来源

推荐依赖获取顺序：

1. `context.Services`
2. Registry / Context 抽象
3. 不再新增对 `Facade` 的依赖

### 4.4 IModuleInstaller

模块通过 `IModuleInstaller` 安装：

```csharp
public interface IModuleInstaller
{
    void Install(IServiceRegistry services);
}
```

模块的职责是：

- 注册服务
- 注册状态
- 注册 View / Controller / Model
- 组织业务能力

模块不应该承担所有事情，应该按职责拆分。

---

## 5. Demo 当前模块划分

`DemoGameModule` 现在只是组合模块：

- `DemoRegistryModule`
- `DemoPresentationModule`
- `DemoRuntimeModule`

### 5.1 DefaultFoundationModule

基础模块，负责注册通用服务，例如：

- `IAssetsLoader`
- `IJUIManager`
- `EventManager`
- `IEventBus`
- `IHttpRequest`
- `IGameObjectManager`
- `IJConfigManager`
- `ISpriteManager`
- `ITransitionProvider`
- `IGameAudioManager`
- `IDataManager`
- `IGameDataStore`

它的定位是“所有业务模块都能复用的基础设施层”。

### 5.2 DemoRegistryModule

负责注册 Demo 的强类型注册表：

- `IModelRegistry`
- `IControllerRegistry`
- `IViewRegistry`
- `ISceneStateRegistry`

这层解决的是“对象怎么被统一查找和组织”。

### 5.3 DemoPresentationModule

负责 Demo 的表现层注册：

- `DemoLoginController`
- `DemoLoginView`

当前 Demo 已经直接把表现层对象注册进强类型 Registry，不再通过旧 Manager 容器做桥接。

### 5.4 DemoRuntimeModule

负责 Demo 运行时装配：

- `GameContext`
- 运行时初始化器

这层解决的是“Demo 应用如何真正被跑起来”。
当前 Demo 已经直接使用 `SceneFlowService`，不再依赖旧 `DemoSM`。

---

## 6. 如何使用这个框架

### 6.1 创建一个应用入口

最小模板如下：

```csharp
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

这就是以后新项目推荐的标准入口。

### 6.2 创建一个业务模块

推荐把一个业务模块拆成：

- `RegistryModule`
- `PresentationModule`
- `RuntimeModule`
- `GameModule`

示例：

```csharp
public sealed class BattleGameModule : IModuleInstaller
{
    private static readonly IModuleInstaller[] modules =
    {
        new BattleRegistryModule(),
        new BattlePresentationModule(),
        new BattleRuntimeModule(),
    };

    public void Install(IServiceRegistry services)
    {
        foreach (var module in modules)
        {
            module.Install(services);
        }
    }
}
```

然后入口里：

```csharp
app = new JAppBuilder()
    .AddModule(new DefaultFoundationModule())
    .AddModule(new BattleGameModule())
    .Build();
```

### 6.3 注册服务

如果你要新增一个基础服务，例如存档、排行榜、背包服务，可以在模块里注册：

```csharp
public void Install(IServiceRegistry services)
{
    services.AddSingleton<IInventoryService>(new InventoryService());
}
```

使用时：

```csharp
var inventory = context.Services.Resolve<IInventoryService>();
```

### 6.4 注册 Model

通过 `IModelRegistry` 注册：

```csharp
public void Install(IServiceRegistry services)
{
    var models = services.Resolve<IModelRegistry>();
    models.Register(new PlayerModel());
}
```

读取：

```csharp
var model = context.Services.Resolve<IModelRegistry>().Get<PlayerModel>();
```

### 6.5 注册 Controller

通过 `IControllerRegistry` 注册：

```csharp
public void Install(IServiceRegistry services)
{
    var controllers = services.Resolve<IControllerRegistry>();
    controllers.Register(new LoginController());
}
```

读取：

```csharp
var controller = context.Services.Resolve<IControllerRegistry>().Get<LoginController>();
```

### 6.6 注册 View

通过 `IViewRegistry` 注册到对应状态：

```csharp
public void Install(IServiceRegistry services)
{
    var views = services.Resolve<IViewRegistry>();
    views.RegisterForScene(typeof(LoginSceneState), new LoginView());
}
```

这样进入对应状态时，就能拿到该状态关联的 View。

---

## 7. 如何编写状态

### 7.1 当前推荐基类

当前建议继续使用：

- `BaseStateAsync`
- `BaseSceneState<TSceneType>`

这两个基类现在已经支持 `Services` 优先。

### 7.2 状态里怎么取依赖

推荐方式：

```csharp
var services = context.Services;
var registry = services.Resolve<IViewRegistry>();
```

如果状态需要切换场景、获取 UI、获取资源，都优先从 `context.Services` 或小 Context 获取。

不要再把新状态写成强依赖旧入口对象的形式。

### 7.3 场景状态的职责建议

场景状态应该只负责：

- 定义进入该状态要做什么
- 组织场景加载
- 组织关联 View 的打开
- 触发必要的业务初始化

不建议把太多全局服务逻辑硬塞进状态类里。

---

## 8. 如何编写 View

当前 `View` 基类已经支持：

- 从 `context.Services` 获取 `EventManager`
- 从 `context.Services` 获取 `IJUIManager`
- 从 `context.Services` 获取 `IGameObjectManager`

所以 View 的建议写法是：

- 继续继承 `View`
- 不直接持有大而全的入口对象
- 通过基类提供的方法拿 UI / 事件 / 对象管理器

例如：

```csharp
panel = GetUIManager().ShowPanel(...);
```

如果你在 View 里需要业务对象，优先从 Registry 拿，而不是继续做字符串查找。

---

## 9. 如何编写 Controller

当前 Controller 仍然是：

```csharp
public abstract class Controller
{
    public abstract Task Do(GameContext context, params object[] parameters);
}
```

新的使用建议是：

- 通过 `context.Services` 获取依赖
- 不假设 `Facade` 一定存在
- 让 Controller 更像“业务动作执行器”

例如 `DemoLoginController` 现在已经是这种风格：

- 从 `context.Services` 获取 `IHttpRequest`
- 从 `context.Services` 获取 `ITransitionProvider`
- 从 `context.Services` 获取 `ISceneFlow`

这就是后续 Controller 的推荐方向。

---

## 10. 如何切场景

当前场景流转的统一抽象是：

- `ISceneFlow`
- `ISceneStateRegistry`
- `ISceneState`

Demo 现在已经直接使用 `ISceneFlow + ISceneStateRegistry` 作为主状态流。

你可以这样理解：

- 对业务开发者来说，应该把“场景切换”看成一种服务能力
- 对框架内部来说，可以继续逐步把旧状态机替换成更强类型的 `SceneFlow`

如果你现在要扩展功能，优先沿着 `ISceneFlow + Registry` 的方向走，不要再新增字符串驱动的入口。

---

## 11. 推荐开发规范

### 11.1 新功能开发

推荐：

- 只使用 `IJApp` 启动链
- 新功能写成模块
- 新依赖注册到 `IServiceRegistry`
- 新业务对象注册到强类型 Registry
- 新状态 / View / Controller 优先依赖 `Services`

不推荐：

- 继续扩写已经移除的旧启动链
- 继续把新逻辑绑定到全能入口对象
- 新增字符串名称查找式接口
- 继续扩大旧 Manager 的职责边界

### 11.2 依赖获取顺序

统一建议：

1. `context.Services`
2. `IModelRegistry / IControllerRegistry / IViewRegistry`
3. 小 Context 抽象
4. 不再新增 `Facade` 依赖

### 11.3 模块划分原则

一个模块应该只解决一类问题：

- Registry 负责注册关系
- Presentation 负责表现层组织
- Runtime 负责运行时装配
- Foundation 负责基础设施

避免把所有注册、初始化、状态跳转、业务逻辑都堆进一个超大模块里。

---

## 12. Legacy 清理结果

旧的 Legacy 兼容层已经从项目中移除。

当前项目中不再保留旧入口、旧兼容桥接和历史拼写接口。

这意味着当前代码库已经完成以下收口：

- 启动只走 `IJApp`
- 状态流转只走 `ISceneFlow`
- 新业务对象只走强类型 Registry
- 旧 `Facade` 兼容链不再参与运行时

后续的清理重点不再是删除 Legacy，而是继续优化主路径本身，例如：

1. 进一步统一历史目录和命名
2. 持续减少旧 `Manager` 风格 API 的对外暴露
3. 把更多业务模块迁成和 `Battle` 一样的纯新模式

---

## 13. 给业务开发者的最短上手指南

如果你现在要基于这个框架开发新内容，最短流程就是：

1. 在场景里挂 `Demo` 或你自己的 `GameEntry`
2. 用 `JAppBuilder` 安装 `DefaultFoundationModule`
3. 再安装你的业务模块
4. 在模块里注册服务、状态、View、Controller、Model
5. 在状态里组织界面打开和业务流程
6. 在 Controller 里处理动作
7. 在 View 里处理表现层交互

一句话概括：

- 应用从 `IJApp` 启动
- 依赖从 `IServiceRegistry` 获取
- 业务对象从强类型 Registry 获取
- 功能通过模块组合

---

## 14. 后续建议

如果接下来继续演进，最有价值的方向是：

- 继续削弱 `Facade` 兼容层
- 把状态流彻底收敛到强类型 `SceneFlow`
- 逐步去掉旧 Manager 的业务入口地位
- 统一历史拼写问题和旧目录命名
- 最终让新项目完全不需要理解旧架构

当前这套 `IJApp + Module + ServiceRegistry + Registry + SceneFlow` 的组合，已经可以作为后续扩展的主干使用。

补充说明：

- 新代码应只使用 `IGameAssetsQuery`
- 旧拼写接口 `IGameAssetsQuary` 已经从项目中移除，统一只使用 `IGameAssetsQuery`

业务开发者如果只需要快速接入，可以直接参考：

- `Assets/com.hiplay.jframework.unity/Runtime/Scripts/QUICK_START.md`

# Dependencies Framework

## Version 2.0.0

- Complete refactor of the framework.

---

## Overview

**Dependencies** is a lightweight, modular dependency injection (DI) framework tailored for Unity. It embraces Unity workflows while supporting powerful DI patterns through context-aware binding, scoped lifetimes, and attribute-driven injection.

---

## Features

- **Multiple Contexts** — Create isolated dependency trees for runtime, testing, or per-scene setups.
- **Constructor & Attribute Injection** — Supports constructor-based and `[Inject]` attribute-based injection.
- **Fluent Binding API** — Define bindings in an expressive and chainable style.
- **Scoped & Singleton Lifetimes** — Easily control when and how often objects are created.
- **Editor Integration Friendly** — Use `ScriptableInstaller` and `SceneContext` for modular, designer-configurable bindings.

---

## Installation

To add this framework to your Unity project:

1. Open your Unity project.

2. Go to `Window > Package Manager`.

3. Click the `+` button and choose `Add package from Git URL...`.

4. Enter:

   ```
   https://github.com/frenchtr/dependencies.git
   ```

5. Click `Add`.

Requires Git to be installed and available in your system path.

---

## Quick Start

### Define a Service

```csharp
public interface ILogger
{
    void Log(string message);
}

public class DebugLogger : ILogger
{
    public void Log(string message)
    {
        Debug.Log(message);
    }
}
```

### Bind the Service

```csharp
public class GameInstaller : MonoInstaller
{
    public override void InstallBindings(IContainer container)
    {
        container.Bind<ILogger>()
                 .To<DebugLogger>()
                 .AsSingleton();
    }
}
```

### Inject the Service

```csharp
public class Player : MonoBehaviour
{
    [Inject] private ILogger logger;

    private void Start()
    {
        logger.Log("Player initialized.");
    }
}
```

---

## API Summary

### Core Types

- `Container` — Central DI entry point. Inherits from `IRegistry`, `IResolver`, and `IInjector`.
- `SceneContext` — Unity MonoBehaviour that initializes a scoped container and injects all scene objects.
- `GlobalContext` — A ScriptableObject that initializes the global container via `RuntimeInitializeOnLoad`.
- `MonoInstaller` / `ScriptableInstaller` — Installers that register bindings to a container.
- `DI` — Static helper for global injection and resolving.

### Key Interfaces

- `IContainer` — Combines registry, resolver, and injector APIs.
- `IBindingBuilder` — Fluent binding API (generic and dynamic).
- `IInjector` — Injects fields, properties, and methods marked with `[Inject]`.
- `IResolver` — Resolves types via constructor or factory.
- `IRegistry` — Registers and looks up bindings.

### Attributes

- `[Inject]` — Marks fields, properties, or methods to be populated via injection.

---

## Usage Examples

### 🟢 Basic Injection

```csharp
public class HealthSystem : MonoBehaviour
{
    [Inject] private IHealthService healthService;

    private void Start()
    {
        int health = healthService.GetHealth();
        Debug.Log($"Health: {health}");
    }
}

public interface IHealthService
{
    int GetHealth();
}

public class DefaultHealthService : IHealthService
{
    public int GetHealth() => 100;
}
```

### 🟠 Constructor Injection

```csharp
public class EnemyAI
{
    private readonly IThreatService threatService;

    public EnemyAI(IThreatService threatService)
    {
        this.threatService = threatService;
    }

    public void Act(IEnumerable<GameObject> targets)
    {
        var target = threatService.GetHighestThreatTarget(targets);
        Debug.Log($"Attacking: {target.name}");
    }
}
```

### 🔵 Scene Setup

- Add a `SceneContext` to your Unity scene.
- Reference MonoInstallers or ScriptableInstallers in its serialized fields.
- All MonoBehaviours and ScriptableObjects in the scene will be injected at load time.

---

## Advanced Features

### Scoped Injection

```csharp
var container = new Container();
var child = new Container(parent: container);

child.Bind<ILogger>().To<DebugLogger>().AsSingleton();
```

### Runtime Factory Binding

```csharp
container.Bind<IPlayerService>()
         .FromFactory(() => new PlayerService("Hero"))
         .AsSingleton();
```

### Switching Installers with Contexts

```csharp
var testContainer = new Container();
testContainer.Bind<ILogger>().To<TestLogger>().AsTransient();
```

---

## License

MIT License. See [LICENSE](LICENSE) for full text.

---

## Contributing

Pull requests and suggestions welcome! Please open an issue or PR on GitHub.

---

## Support

File issues on GitHub: [GitHub Issues](https://github.com/frenchtr/dependencies/issues)


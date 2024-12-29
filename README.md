
# Dependencies

A dependency injection framework for Unity, hand-crafted with love.

This framework provides a powerful and flexible Dependency Injection solution tailored for Unity, minimizing developer overhead while maximizing adaptability. Designed with experienced Unity developers in mind, it integrates seamlessly into Unity projects and supports a wide range of use cases.

---

## Features

- **Multiple Context Support**: Handle different contexts such as runtime environments and testing scenarios effortlessly.
- **Scoped Dependency Management**: Easily manage lifetimes and scopes for your dependencies.
- **Fluent Binding API**: Define and register dependencies with a concise, intuitive syntax.
- **Attribute-Based Injection**: Automatically resolve dependencies with attributes like `[Inject]` for streamlined workflows.

---

## Installation

To add this framework to your Unity project:

1. Open your Unity project.

2. Navigate to `Window > Package Manager`.

3. Click the `+` button and select `Add package from git URL...`.

4. Enter the following Git URL:

   ```
   https://github.com/YourRepo/DependencyInjectionFramework.git
   ```

5. Click `Add`.

Ensure you have Git installed and configured on your system to fetch the package successfully.

---

## API Reference

### Core Classes

`Container`

- Purpose: Acts as the main hub for managing bindings and resolving dependencies.

- Key Methods:
  - `Register(Func<IBindingBuilder, IBinding>)`: Registers a dependency binding.
  - `Resolve<T>()`: Resolves and returns an instance of the specified type.
  - `Inject<T>(T target)`: Injects dependencies into the specified object.`
  
`Context`
- Purpose: Provides a scoped environment for specific dependency bindings.
- Key Methods:
  - `Bind<T>()`: Binds a type to an implementation within the context.
  - `Dispose()`: Cleans up the context when no longer needed.

`Dependencies`
- Purpose: Acts as the global entry point for initializing and accessing the framework.
- Key Methods:
  - `Initialize(Container)`: Initializes the framework with the specified container.
  - `SetContext(Context)`: Sets the active context for dependency resolution.

`Scope`

- Purpose: Manages the lifecycle of scoped dependencies.
- Key Methods:
  - `Resolve<T>()`: Resolves a dependency within the scope.

### Attributes

`[Inject]`

- Purpose: Marks fields or properties to be injected with dependencies.

### Exceptions

`DependencyException`
- Purpose: Base class for all exceptions related to dependency injection.

`ResolveException`
- Purpose: Thrown when a dependency cannot be resolved.

`ConstructException`
- Purpose: Thrown when an object cannot be constructed.

---

## Quick Start Guide

Here’s a basic example to help you get started:

### 1. Define Your Dependencies

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

### 2. Configure Bindings

Create a script to set up bindings for your dependencies:

```csharp
public class GameBindings : MonoBehaviour
{
    private void Awake()
    {
        var container = new Container();

        container.Register(builder => builder
            .Bind<ILogger>()
            .To<DebugLogger>()
            .AsSingleton());

        Dependencies.Initialize(container);
    }
}
```

### 3. Inject Dependencies

Use the `[Inject]` attribute to resolve dependencies automatically:

```csharp
public class SampleConsumer : MonoBehaviour
{
    [Inject]
    private ILogger logger;

    private void Start()
    {
        logger.Log("Dependency Injection is working!");
    }
}
```

---

## Usage Examples

### Basic Example: Simple Dependency Injection

```csharp
public interface IMessageService
{
    void SendMessage(string message);
}

public class MessageService : IMessageService
{
    public void SendMessage(string message)
    {
        Debug.Log($"Message sent: {message}");
    }
}

public class BasicExample : MonoBehaviour
{
    [Inject]
    private IMessageService messageService;

    private void Start()
    {
        messageService.SendMessage("Hello, World!");
    }
}

public class BasicBindings : MonoBehaviour
{
    private void Awake()
    {
        var container = new Container();

        container.Register(builder => builder
            .Bind<IMessageService>()
            .To<MessageService>()
            .AsSingleton());
    }
}
```

### Intermediate Example: Constructor Injection

```csharp
public interface IEmailService
{
    void SendEmail(string recipient, string subject);
}

public class EmailService : IEmailService
{
    public void SendEmail(string recipient, string subject)
    {
        Debug.Log($"Email sent to {recipient} with subject: {subject}");
    }
}

public class NotificationService
{
    private readonly IEmailService emailService;

    public NotificationService(IEmailService emailService)
    {
        this.emailService = emailService;
    }

    public void Notify(string recipient, string subject)
    {
        emailService.SendEmail(recipient, subject);
    }
}

public class IntermediateBindings : MonoBehaviour
{
    private void Awake()
    {
        var container = new Container();

        container.Register(builder => builder
            .Bind<IEmailService>()
            .To<EmailService>()
            .AsSingleton());

        container.Register(builder => builder
            .Bind<NotificationService>()
            .ToSelf()
            .AsTransient());

        Dependencies.Initialize(container);
    }
}
```

### Advanced Example: Scoped Dependencies and Multiple Contexts

```csharp
public class GameContext : MonoBehaviour
{
    private void Awake()
    {
        var runtimeContext = new Context();

        runtimeContext.Bind<ILogger>().To<DebugLogger>().AsSingleton();
        Dependencies.SetContext(runtimeContext);
    }
}

public class PlayerService
{
    private readonly ILogger logger;

    [Inject]
    public PlayerService(ILogger logger)
    {
        this.logger = logger;
    }

    public void LogPlayerAction(string action)
    {
        logger.Log($"Player action: {action}");
    }
}

public class AdvancedExample : MonoBehaviour
{
    private void Start()
    {
        using (var scope = Dependencies.CreateScope())
        {
            var playerService = scope.Resolve<PlayerService>();
            playerService.LogPlayerAction("Jump");
        }
    }
}
```

---

## Advanced Features

### Multiple Contexts

Easily switch between contexts, such as runtime and testing:

```csharp
var context = new Context();
context.Bind<ILogger>().To<TestLogger>().AsTransient();
Dependencies.SetContext(context);
```

### Scoped Dependencies

Manage object lifetimes effectively by defining scopes:

```csharp
using (var scope = Dependencies.CreateScope())
{
    var logger = scope.Resolve<ILogger>();
    logger.Log("Scoped Dependency");
}
```

---

## Versioning

This framework follows [Semantic Versioning (SemVer)](https://semver.org/). Updates will be released as needed.

---

## Support

For issues or feature requests, please create a ticket on the [GitHub issues page](https://github.com/YourRepo/DependencyInjectionFramework/issues).

---

## Contributing

Contributions are welcome! Fork the repository, make changes, and submit a pull request for review.

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

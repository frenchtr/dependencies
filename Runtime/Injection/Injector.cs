using System;
using System.Linq;
using System.Reflection;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Resolution;

namespace TravisRFrench.Dependencies.Injection
{
	/// <summary>
	/// Default implementation of <see cref="IInjector"/> that performs reflection-based injection
	/// into fields, properties, and methods marked with <see cref="InjectAttribute"/>.
	/// </summary>
	public class Injector : IInjector
	{
		private readonly IContainer container;

		private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		/// <summary>
		/// Creates a new injector using the given container for resolution.
		/// </summary>
		/// <param name="container">The container to resolve dependencies from.</param>
		public Injector(IContainer container)
		{
			this.container = container;
		}

		/// <inheritdoc/>
		public void Inject(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}
			
			try
			{
				var type = obj.GetType();
				this.InjectFields(obj, type);
				this.InjectProperties(obj, type);
				this.InjectMethods(obj, type);
			}
			catch (Exception exception)
			{
				throw new InjectionException(this, $"Failed to inject object of type '{obj.GetType().Name}'.", exception);
			}
		}

		public object Instantiate(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}
			
			try
			{
				var constructors = type
					.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
					.OrderBy(c => c.GetParameters().Length);

				foreach (var constructor in constructors)
				{
					var parameters = constructor.GetParameters();
					var arguments = new object[parameters.Length];

					try
					{
						for (var index = 0; index < parameters.Length; index++)
						{
							var parameter = parameters[index];

							var context = new InjectionContext()
							{
								InjectedObjectType = type,
								InjectionMode = InjectionMode.Constructor,
								TargetConstructor = constructor,
								ParameterType = parameter.ParameterType,
								ParameterName = parameter.Name,
								InjectedName = parameter.Name,
							};
							
							var argument = this.container.Resolve(parameter.ParameterType, context);
							arguments[index] = argument;
						}

						var instance = constructor.Invoke(arguments);
						this.Inject(instance);

						return instance;
					}
					catch
					{
						continue;
					}
				}
				
				throw new InvalidOperationException($"No suitable constructor found for type {type.Name}");
			}
			catch (Exception exception)
			{
				throw new ConstructorCreationException(type, $"Unable to create instance from constructor.", exception);
			}
		}

		public object InstantiateFromFactory(Func<object> factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException(nameof(factory));
			}

			var factoryType = factory.GetType();
			var type = factoryType.GetGenericArguments()
				.First();
			
			try
			{
				var instance = factory.Invoke();
				this.Inject(instance);
				
				return instance;
			}
			catch (Exception exception)
			{
				throw new FactoryCreationException(type, $"Unable to create instance from factory.", exception);
			}
		}

		private void InjectFields(object obj, Type type)
		{
			var fields = this.GetInjectableFields(type);

			foreach (var field in fields)
			{
				try
				{
					var context = new InjectionContext()
					{
						InjectionMode = InjectionMode.Field,
						InjectedObjectType = type,
						InjectedObjectInstance = obj,
						MemberName = field.Name,
						MemberType = field.FieldType,
						TargetMember = field,
						TargetField = field,
						InjectedName = field.Name,
					};
				
					var value = this.container.Resolve(field.FieldType, context);
					field.SetValue(obj, value);
				}
				catch (Exception exception)
				{
					throw new FieldInjectionException(this, field, $"Failed to inject field '{field.Name}' of type {field.FieldType}.", exception);
				}
			}
		}

		private void InjectProperties(object obj, Type type)
		{
			var properties = this.GetInjectableProperties(type);

			foreach (var property in properties)
			{
				try
				{
					var context = new InjectionContext()
					{
						InjectionMode = InjectionMode.Property,
						InjectedObjectType = type,
						InjectedObjectInstance = obj,
						MemberName = property.Name,
						MemberType = property.PropertyType,
						TargetMember = property,
						TargetProperty = property,
						InjectedName = property.Name,
					};
				
					var value = this.container.Resolve(property.PropertyType, context);
					property.SetValue(obj, value);
				}
				catch (Exception exception)
				{
					throw new PropertyInjectionException(this, property, $"Failed to inject property '{property.Name}' of type '{property.PropertyType}'.", exception);
				}
			}
		}

		private void InjectMethods(object obj, Type type)
		{
			var methods = this.GetInjectableMethods(type);

			foreach (var method in methods)
			{
				try
				{
					var parameters = method.GetParameters();
					var arguments = new object[parameters.Length];

					for (var i = 0; i < parameters.Length; i++)
					{
						var parameter = parameters[i];
					
						try
						{
							var context = new InjectionContext()
							{
								InjectionMode = InjectionMode.Method,
								InjectedObjectType = type,
								InjectedObjectInstance = obj,
								MemberName = method.Name,
								MemberType = method.ReturnType,
								TargetMember = method,
								TargetMethod = method,
								TargetParameter = parameter,
								ParameterName = parameter.Name,
								ParameterType = parameter.ParameterType,
								InjectedName = parameter.Name,
							};
					
							var parameterType = parameter.ParameterType;
							arguments[i] = this.container.Resolve(parameterType, context);
						}
						catch (Exception exception)
						{
							throw new ParameterInjectionException(this, parameter, $"Failed to inject parameter '{parameter.Name}' of type '{parameter.ParameterType}'.", exception);
						}
					}

					method.Invoke(obj, arguments);
				}
				catch (Exception exception)
				{
					throw new MethodInjectionException(this, method, $"Failed to inject method '{method.Name}'.", exception);
				}
			}
		}

		private FieldInfo[] GetInjectableFields(Type type)
		{
			return type.GetFields(Flags)
				.Where(f => f.IsDefined(typeof(InjectAttribute)))
				.ToArray();
		}

		private PropertyInfo[] GetInjectableProperties(Type type)
		{
			return type.GetProperties(Flags)
				.Where(p => p.IsDefined(typeof(InjectAttribute)))
				.ToArray();
		}

		private MethodInfo[] GetInjectableMethods(Type type)
		{
			return type.GetMethods(Flags)
				.Where(m => m.IsDefined(typeof(InjectAttribute)))
				.ToArray();
		}
	}
}

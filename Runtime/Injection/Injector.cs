using System;
using System.Linq;
using System.Reflection;
using TravisRFrench.Dependencies.Containers;

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
			var type = obj.GetType();
			this.InjectFields(obj, type);
			this.InjectProperties(obj, type);
			this.InjectMethods(obj, type);
		}

		private void InjectFields(object obj, Type type)
		{
			var fields = this.GetInjectableFields(type);

			foreach (var field in fields)
			{
				var value = this.container.Resolve(field.FieldType);
				field.SetValue(obj, value);
			}
		}

		private void InjectProperties(object obj, Type type)
		{
			var properties = this.GetInjectableProperties(type);

			foreach (var property in properties)
			{
				var value = this.container.Resolve(property.PropertyType);
				property.SetValue(obj, value);
			}
		}

		private void InjectMethods(object obj, Type type)
		{
			var methods = this.GetInjectableMethods(type);

			foreach (var method in methods)
			{
				var parameters = method.GetParameters();
				var arguments = new object[parameters.Length];

				for (var i = 0; i < parameters.Length; i++)
				{
					var parameterType = parameters[i].ParameterType;
					arguments[i] = this.container.Resolve(parameterType);
				}

				method.Invoke(obj, arguments);
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

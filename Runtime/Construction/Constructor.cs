using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TravisRFrench.Dependencies.Runtime.Injection;
using TravisRFrench.Dependencies.Runtime.Resolution;
using TravisRFrench.Dependencies.Runtime.Scopes;

namespace TravisRFrench.Dependencies.Runtime.Construction
{
    public class Constructor : IConstructor
    {
        private readonly IScope scope;
        private readonly IResolver resolver;

        public Constructor(IScope scope, IResolver resolver)
        {
            this.scope = scope;
            this.resolver = resolver;
        }
        
        public object Construct(Type type)
        {
            var constructor = this.GetBestConstructor(type);
            var parameters = constructor.GetParameters();
            var arguments = this.ResolveArguments(parameters);

            return constructor.Invoke(arguments);
        }

        private ConstructorInfo GetBestConstructor(Type type)
        {
            var constructors = type.GetConstructors();

            // Step 1: Filter constructors where all parameters are resolvable
            var resolvableConstructors = constructors.Where(constructor =>
                    constructor.GetParameters()
                        .All(parameter => this.CanResolve(parameter.ParameterType)))
                .ToList();

            if (!resolvableConstructors.Any())
            {
                throw new InvalidOperationException($"No suitable constructor found for type {type.FullName}.");
            }

            // Step 2: Prioritize constructors with [Inject] attribute
            var injectConstructors = resolvableConstructors
                .Where(constructor => constructor.IsDefined(typeof(InjectAttribute), false))
                .ToList();

            if (injectConstructors.Any())
            {
                // If [Inject] constructors exist, prioritize by number of parameters (fewer is better)
                return GetConstructorWithFewestParameters(injectConstructors);
            }

            // Step 3: If no [Inject] constructors, prioritize by number of parameters (fewer is better)
            return GetConstructorWithFewestParameters(resolvableConstructors);

            ConstructorInfo GetConstructorWithFewestParameters(IEnumerable<ConstructorInfo> constructorEnumerable)
            {
                return constructorEnumerable
                    .OrderBy(constructor => constructor.GetParameters().Length)
                    .First();
            }
        }

        private bool CanResolve(Type parameterType)
        {
            // Check if the parameter type can be resolved from the container
            return this.scope.Get(parameterType) != null;
        }
        
        private object[] ResolveArguments(ParameterInfo[] parameters)
        {
            var arguments = new object[parameters.Length];
            
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                arguments[i] = this.resolver.Resolve(parameter.ParameterType);
            }
            
            return arguments;
        }
    }
}

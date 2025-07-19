using System;
using NUnit.Framework;
using TravisRFrench.Dependencies.Bindings;
using TravisRFrench.Dependencies.Containers;

namespace TravisRFrench.Dependencies.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="Container"/> class and its resolution behavior.
    /// </summary>
    public class ContainerTests
    {
        public interface IService { }

        public class Service : IService { }

        /// <summary>
        /// Ensures that a registered type can be resolved to an instance.
        /// </summary>
        [Test]
        public void GivenRegisteredType_WhenResolved_ThenItShouldReturnInstance()
        {
            // ARRANGE
            IContainer container = new Container();
            var binding = new Binding(typeof(IService), typeof(Service));
            container.Register(binding);

            // ACT
            var instance = container.Resolve(typeof(IService));

            // ASSERT
            Assert.IsNotNull(instance);
            Assert.IsInstanceOf<Service>(instance);
        }

        /// <summary>
        /// Ensures that a registered instance is returned as-is on resolve.
        /// </summary>
        [Test]
        public void GivenTypeRegisteredFromInstance_WhenResolved_ThenItShouldReturnInstance()
        {
            // ARRANGE
            var service = new Service();
            IContainer container = new Container();
            var binding = new Binding(typeof(IService), typeof(Service), service);
            container.Register(binding);

            // ACT
            var instance = container.Resolve(typeof(IService));

            // ASSERT
            Assert.IsNotNull(instance);
            Assert.IsInstanceOf<Service>(instance);
            Assert.AreSame(service, instance);
        }

        /// <summary>
        /// Ensures that resolving an unregistered type throws an exception.
        /// </summary>
        [Test]
        public void GivenUnregisteredType_WhenResolved_ThenItShouldThrow()
        {
            // ARRANGE
            IContainer container = new Container();

            // ACT & ASSERT
            Assert.Throws<InvalidOperationException>(() => container.Resolve(typeof(IService)));
        }

        /// <summary>
        /// Ensures singleton bindings return the same instance every time.
        /// </summary>
        [Test]
        public void GivenSingletonRegistration_WhenResolvedMultipleTimes_ThenItShouldReturnSameInstance()
        {
            // ARRANGE
            IContainer container = new Container();
            var binding = new Binding(typeof(IService), typeof(Service), Lifetime.Singleton);
            container.Register(binding);

            // ACT
            var first = container.Resolve(typeof(IService));
            var second = container.Resolve(typeof(IService));

            // ASSERT
            Assert.AreSame(first, second);
        }

        /// <summary>
        /// Ensures transient bindings return a new instance every time.
        /// </summary>
        [Test]
        public void GivenTransientRegistration_WhenResolvedMultipleTimes_ThenItShouldReturnDifferentInstances()
        {
            // ARRANGE
            IContainer container = new Container();
            var binding = new Binding(typeof(IService), typeof(Service), Lifetime.Transient);
            container.Register(binding);

            // ACT
            var first = container.Resolve(typeof(IService));
            var second = container.Resolve(typeof(IService));

            // ASSERT
            Assert.AreNotSame(first, second);
        }

        /// <summary>
        /// Ensures that after unregistering, the service can no longer be resolved.
        /// </summary>
        [Test]
        public void GivenRegisteredService_WhenUnregistered_ThenItShouldNotBeResolvable()
        {
            // ARRANGE
            IContainer container = new Container();
            var binding = new Binding(typeof(IService), typeof(Service));
            container.Register(binding);
            container.Unregister(typeof(IService));

            // ACT & ASSERT
            Assert.Throws<InvalidOperationException>(() => container.Resolve(typeof(IService)));
        }
    }
}

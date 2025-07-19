using System;
using NUnit.Framework;
using TravisRFrench.Dependencies.Bindings;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Injection;

namespace TravisRFrench.Dependencies.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="BindingBuilder"/> and related generic types.
    /// </summary>
    public class BindingBuilderTests
    {
        public interface ITestService {}
        public class TestService : ITestService {}
        public abstract class AbstractService : ITestService {}

        public class FakeContainer : IContainer
        {
            public IBinding LatestBinding;
            private IContainer parent;

            public void Register(IBinding binding) => LatestBinding = binding;
            public void Unregister(Type interfaceType) {}
            public bool TryGetBinding(Type interfaceType, out IBinding binding, IInjectionContext context = null)
            {
                binding = null;
                return false;
            }
            public object Resolve(Type interfaceType, IInjectionContext context = null) => throw new NotImplementedException();
            public void Inject(object obj) => throw new NotImplementedException();
            public IContainer Parent => this.parent;
            public T Instantiate<T>() => throw new NotImplementedException();
        }

        /// <summary>
        /// Ensures that calling To&lt;TImplementation&gt;() sets the correct implementation type.
        /// </summary>
        [Test]
        public void GivenGenericBuilder_WhenToIsCalled_ThenSetsImplementationType()
        {
            /* GIVEN */
            var container = new FakeContainer();
            var builder = new BindingBuilder<ITestService>(container);

            /* WHEN */
            var implBuilder = builder.To<TestService>();

            /* THEN */
            Assert.AreEqual(typeof(ITestService), implBuilder.InterfaceType);
            Assert.AreEqual(typeof(TestService), implBuilder.ImplementationType);
        }

        /// <summary>
        /// Ensures that ToSelf() registers a type binding to itself correctly.
        /// </summary>
        [Test]
        public void GivenGenericBindingBuilder_WhenToSelfIsCalledWithConcreteType_ThenRegistersCorrectly()
        {
            /* GIVEN */
            var container = new FakeContainer();
            var builder = new BindingBuilder<TestService>(container);

            /* WHEN */
            var implBuilder = builder
                .ToSelf()
                .FromNew();

            /* THEN */
            Assert.AreEqual(typeof(TestService), container.LatestBinding.InterfaceType);
            Assert.AreEqual(typeof(TestService), container.LatestBinding.ImplementationType);
        }

        /// <summary>
        /// Ensures that calling AsSingleton sets the lifetime correctly.
        /// </summary>
        [Test]
        public void GivenGenericBindingBuilder_WhenAsSingletonIsCalled_ThenSetsLifetimeToSingleton()
        {
            /* GIVEN */
            var container = new FakeContainer();
            var builder = new BindingBuilder<ITestService>(container)
                .To<TestService>();

            /* WHEN */
            builder.AsSingleton();

            /* THEN */
            Assert.AreEqual(Lifetime.Singleton, builder.Lifetime);
        }

        /// <summary>
        /// Ensures that FromNew registers a new instance construction source.
        /// </summary>
        [Test]
        public void GivenImplementationBuilder_WhenFromNewIsCalled_ThenRegistersNewInstanceBinding()
        {
            /* GIVEN */
            var container = new FakeContainer();
            var builder = new BindingBuilder<ITestService>(container).To<TestService>();

            /* WHEN */
            builder.FromNew();

            /* THEN */
            var binding = container.LatestBinding;
            Assert.AreEqual(ConstructionSource.FromNew, binding.Source);
        }

        /// <summary>
        /// Ensures that FromInstance registers a static instance correctly.
        /// </summary>
        [Test]
        public void GivenImplementationBuilder_WhenFromInstanceIsCalled_ThenRegistersInstanceBinding()
        {
            /* GIVEN */
            var container = new FakeContainer();
            var instance = new TestService();
            var builder = new BindingBuilder<ITestService>(container).To<TestService>();

            /* WHEN */
            builder.FromInstance(instance);

            /* THEN */
            var binding = container.LatestBinding;
            Assert.AreEqual(ConstructionSource.FromInstance, binding.Source);
            Assert.AreEqual(instance, binding.Instance);
        }

        /// <summary>
        /// Ensures that FromFactory registers a factory method correctly.
        /// </summary>
        [Test]
        public void GivenImplementationBuilder_WhenFromFactoryIsCalled_ThenRegistersFactoryBinding()
        {
            /* GIVEN */
            var container = new FakeContainer();
            TestService Factory() => new();
            var builder = new BindingBuilder<ITestService>(container)
                .To<TestService>();

            /* WHEN */
            builder.FromFactory(Factory);

            /* THEN */
            var binding = container.LatestBinding;
            Assert.AreEqual(ConstructionSource.FromFactory, binding.Source);
            Assert.NotNull(binding.Factory);
        }

        /// <summary>
        /// Ensures that ToSelf works correctly for a non-generic binding builder.
        /// </summary>
        [Test]
        public void GivenDynamicBindingBuilder_WhenToSelfIsCalledWithConcreteClass_ThenRegistersBindingCorrectly()
        {
            /* GIVEN */
            var container = new FakeContainer();
            var builder = new BindingBuilder(container, typeof(TestService));

            /* WHEN */
            builder.ToSelf().FromNew();

            /* THEN */
            var binding = container.LatestBinding;
            Assert.AreEqual(typeof(TestService), binding.InterfaceType);
            Assert.AreEqual(typeof(TestService), binding.ImplementationType);
        }
        
        [Test]
        public void GivenConditionalFieldBinding_WhenFieldMatches_ThenBindingIsUsed()
        {
            // GIVEN
            var container = new Container();
            var expected = new Camera();

            container.Bind<Camera>()
                .ToSelf()
                .FromInstance(expected)
                .When(ctx => ctx.MemberName == "mainCamera");

            var obj = new ClassWithMainCameraField();

            // WHEN
            container.Inject(obj);

            // THEN
            Assert.That(obj.mainCamera, Is.SameAs(expected));
        }

        [Test]
        public void GivenConditionalFieldBinding_WhenFieldDoesNotMatch_ThenBindingIsNotUsed()
        {
            // GIVEN
            var container = new Container();
            var expected = new Camera();

            container.Bind<Camera>()
                .ToSelf()
                .FromInstance(expected)
                .When(ctx => ctx.MemberName == "mainCamera");

            var obj = new ClassWithOtherCameraField();

            // WHEN / THEN
            Assert.IsNull(obj.someOtherCamera);
        }

        [Test]
        public void GivenConditionalBinding_WhenResolvedManually_ThenBindingIsNotUsed()
        {
            // GIVEN
            var container = new Container();
            container.Bind<Camera>()
                .ToSelf()
                .FromInstance(new Camera())
                .When(ctx => ctx.MemberName == "mainCamera");

            // WHEN / THEN
            Assert.Throws<InvalidOperationException>(() => container.Resolve<Camera>());
        }

        // -------------------------
        // Support Types for Testing
        // -------------------------

        public class Camera { }

        public class ClassWithMainCameraField
        {
            [Inject]
            public Camera mainCamera;
        }

        public class ClassWithOtherCameraField
        {
            [Inject]
            public Camera someOtherCamera;
        }
    }
}

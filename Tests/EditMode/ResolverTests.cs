using NUnit.Framework;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Injection;
using TravisRFrench.Dependencies.Resolution;

namespace TravisRFrench.Dependencies.Tests
{
	[TestFixture]
	public class ResolverTests
	{
		private IContainer container;

		[SetUp]
		public void Setup()
		{
			this.container = new Container();
		}

		[TearDown]
		public void Teardown()
		{
		}

		[Test]
		[Category("Integration")]
		public void GivenABindingDeclaredFromNew_WhenResolved_ThenItShouldReturnAnInstance()
		{
			/* GIVEN */
			this.container.Bind<ITestServiceA>()
				.To<TestServiceA>()
				.FromNew()
				.AsSingleton();

			/* WHEN */
			var instance = this.container.Resolve<ITestServiceA>();

			/* THEN */
			Assert.IsNotNull(instance);
			Assert.IsInstanceOf<TestServiceA>(instance);
		}
		
		[Test]
		[Category("Integration")]
		public void GivenABindingDeclaredAsTransient_WhenResolvedMultipleTimes_ThenItShouldReturnDifferentInstances()
		{
			/* GIVEN */
			this.container.Bind<ITestServiceA>()
				.To<TestServiceA>()
				.AsTransient();

			/* WHEN */
			var first = this.container.Resolve<ITestServiceA>();
			var second = this.container.Resolve<ITestServiceA>();

			/* THEN */
			Assert.IsNotNull(first);
			Assert.IsNotNull(second);
			Assert.AreNotSame(first, second);
		}
		
		[Test]
		[Category("Integration")]
		public void GivenABindingDeclaredAsSingleton_WhenResolvedMultipleTimes_ThenItShouldReturnTheSameInstance()
		{
			/* GIVEN */
			this.container.Bind<ITestServiceA>()
				.To<TestServiceA>()
				.AsSingleton();

			/* WHEN */
			var first = this.container.Resolve<ITestServiceA>();
			var second = this.container.Resolve<ITestServiceA>();

			/* THEN */
			Assert.IsNotNull(first);
			Assert.IsNotNull(second);
			Assert.AreSame(first, second);
		}

		[Test]
		[Category("Integration")]
		public void GivenABindingDeclaredFromInstance_WhenResolved_ThenItShouldReturnTheInstance()
		{
			/* GIVEN */
			var service = new TestServiceA();
			this.container.Bind<ITestServiceA>()
				.To<TestServiceA>()
				.FromInstance(service)
				.AsSingleton();

			/* WHEN */
			var instance = this.container.Resolve<ITestServiceA>();

			/* THEN */
			Assert.IsNotNull(instance);
			Assert.AreSame(instance, service);
		}

		[Test]
		[Category("Integration")]
		public void GivenAnUndeclaredType_WhenResolved_ThenItShouldThrowBindingNotFoundException()
		{
			/* GIVEN */
			/* WHEN */
			void Resolve()
			{
				this.container.Resolve<ITestServiceA>();
			}

			/* THEN */
			var exception = Assert.Throws<TypeResolutionException>(Resolve);
			Assert.IsInstanceOf<BindingNotFoundException>(exception.InnerException);
		}

		[Test]
		[Category("Integration")]
		public void GivenABindingDeclaredWithCondition_WhenResolvedManually_ThenItShouldThrowBindingNotFoundException()
		{
			/* GIVEN */
			this.container.Bind<ITestServiceA>()
				.To<TestServiceA>()
				.When(ctx => true);
			
			/* WHEN */
			void Resolve()
			{
				this.container.Resolve<ITestServiceA>();
			}

			/* THEN */
			var exception = Assert.Throws<TypeResolutionException>(Resolve);
			Assert.IsInstanceOf<BindingNotFoundException>(exception.InnerException);
		}
		
		[Test]
		[Category("Integration")]
		public void GivenARegisteredTypeThatHasDependencies_WhenResolved_ThenAllDependenciesShouldBeProvided()
		{
			/* GIVEN */
			this.container.Bind<ITestServiceA>()
				.To<TestServiceA>()
				.FromNew()
				.AsSingleton();

			this.container.Bind<ITestServiceB>()
				.To<TestServiceB>()
				.FromNew()
				.AsSingleton();

			this.container.Bind<ITestServiceC>()
				.To<TestServiceC>()
				.FromNew()
				.AsSingleton();

			this.container.Bind<ITestServiceD>()
				.To<TestServiceD>()
				.FromNew()
				.AsSingleton();

			/* WHEN */
			var instance = this.container.Resolve<ITestServiceD>();

			/* THEN */
			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.TestServiceA, nameof(instance.TestServiceA));
			Assert.IsNotNull(instance.TestServiceB, nameof(instance.TestServiceB));
			Assert.IsNotNull(instance.TestServiceC, nameof(instance.TestServiceC));
		}
		
		private interface ITestServiceA
		{
		}

		private interface ITestServiceB
		{
			ITestServiceA TestServiceA { get; }
		}

		private interface ITestServiceC
		{
			ITestServiceA TestServiceA { get; }
			ITestServiceB TestServiceB { get; }
		}

		private interface ITestServiceD
		{
			ITestServiceA TestServiceA { get; }
			ITestServiceB TestServiceB { get; }
			ITestServiceC TestServiceC { get; }
		}
		
		private class TestServiceA : ITestServiceA
		{
		}

		private class TestServiceB : ITestServiceB
		{
			public ITestServiceA TestServiceA { get; }

			public TestServiceB(ITestServiceA testServiceA)
			{
				this.TestServiceA = testServiceA;
			}
		}

		private class TestServiceC : ITestServiceC
		{
			public ITestServiceA TestServiceA { get; }
			public ITestServiceB TestServiceB { get; }
			
			public TestServiceC(ITestServiceA testServiceA, ITestServiceB testServiceB)
			{
				this.TestServiceA = testServiceA;
				this.TestServiceB = testServiceB;
			}
		}

		private class TestServiceD : ITestServiceD
		{
			public ITestServiceA TestServiceA { get; }
			public ITestServiceB TestServiceB { get; }
			[Inject]
			public ITestServiceC TestServiceC { get; set; }
			
			public TestServiceD(ITestServiceA testServiceA, ITestServiceB testServiceB)
			{
				this.TestServiceA = testServiceA;
				this.TestServiceB = testServiceB;
			}
		}
	}
}

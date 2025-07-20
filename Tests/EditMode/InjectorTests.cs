using NUnit.Framework;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Injection;

namespace TravisRFrench.Dependencies.Tests
{
	[TestFixture]
	public class InjectorTests
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
		public void GivenObjectWithAllDependenciesDeclared_WhenInjected_ThenObjectIsFullyInjected()
		{
			/* GIVEN */
			var testServiceA = new TestServiceA();
			var testServiceB = new TestServiceB();
			var testServiceC = new TestServiceC();
			
			this.container.Bind<ITestServiceA>()
				.To<TestServiceA>()
				.FromInstance(testServiceA)
				.AsSingleton();

			this.container.Bind<ITestServiceB>()
				.To<TestServiceB>()
				.FromInstance(testServiceB)
				.AsSingleton();

			this.container.Bind<ITestServiceC>()
				.To<TestServiceC>()
				.FromInstance(testServiceC)
				.AsSingleton();

			var injectable = new InjectableObject();

			/* WHEN */
			this.container.Inject(injectable);
			
			/* THEN */
			Assert.NotNull(injectable.TestServiceA);
			Assert.AreSame(testServiceA, injectable.TestServiceA);
			Assert.NotNull(injectable.TestServiceB);
			Assert.AreSame(testServiceB, injectable.TestServiceB);
			Assert.NotNull(injectable.TestServiceC);
			Assert.AreSame(testServiceC, injectable.TestServiceC);
		}

		public class InjectableObject
		{
			[Inject]
			private ITestServiceA testServiceA;
			private ITestServiceB testServiceB;
			private ITestServiceC testServiceC;

			public ITestServiceA TestServiceA => this.testServiceA;
			
			[Inject]
			public ITestServiceB TestServiceB
			{
				get => this.testServiceB;
				private set => this.testServiceB = value;
			}

			public ITestServiceC TestServiceC => this.testServiceC;

			[Inject]
			public void Inject(ITestServiceC testServiceC)
			{
				this.testServiceC = testServiceC;
			}
		}
		
		public interface ITestServiceA
		{
		}

		public interface ITestServiceB
		{
		}

		public interface ITestServiceC
		{
		}

		public class TestServiceA : ITestServiceA
		{
		}

		public class TestServiceB : ITestServiceB
		{
		}

		public class TestServiceC : ITestServiceC
		{
		}
	}
}

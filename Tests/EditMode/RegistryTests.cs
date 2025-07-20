using System;
using NUnit.Framework;
using TravisRFrench.Dependencies.Bindings;
using TravisRFrench.Dependencies.Containers;

namespace TravisRFrench.Dependencies.Tests
{
	[TestFixture]
	public class RegistryTests
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
		public void GivenEmptyContainer_WhenBindingIsRegistered_ThenItShouldBeInContainer()
		{
			/* GIVEN */
			var expectedBinding = new Binding(typeof(ITestServiceA), typeof(TestServiceA));
			
			/* WHEN */
			this.container.Register(expectedBinding);
			
			/* THEN */
			this.container.TryGetBinding<ITestServiceA>(out var actualBinding);
			
			Assert.IsNotNull(actualBinding);
			Assert.AreSame(expectedBinding, actualBinding);
		}
		
		[Test]
		[Category("Integration")]
		public void GivenRegisteredBinding_WhenBindingIsUnregistered_ThenItShouldNotBeInContainer()
		{
			/* GIVEN */
			var registeredBinding = new Binding(typeof(ITestServiceA), typeof(TestServiceA));
			this.container.Register(registeredBinding);
			
			/* WHEN */
			this.container.Unregister<ITestServiceA>();
			
			/* THEN */
			this.container.TryGetBinding<ITestServiceA>(out var actualBinding);
			
			Assert.IsNull(actualBinding);
		}

		[Test]
		[Category("Integration")]
		public void GivenUnregisteredBinding_WhenTryGetBindingIsCalled_ThenItShouldReturnFalse()
		{
			/* GIVEN */
			/* WHEN */
			var result = this.container.TryGetBinding<ITestServiceA>(out var actualBinding);
			
			/* THEN */
			Assert.IsFalse(result);
			Assert.IsNull(actualBinding);
		}
		
		[Test]
		[Category("Integration")]
		public void GivenRegisteredBindingWithNoCondition_WhenTryGetBindingIsCalled_ThenItShouldReturnTrue()
		{
			/* GIVEN */
			this.container.Bind<ITestServiceA>()
				.To<TestServiceA>()
				.FromNew()
				.AsSingleton();
			
			/* WHEN */
			var result = this.container.TryGetBinding<ITestServiceA>(out var actualBinding);
			
			/* THEN */
			Assert.IsTrue(result);
			Assert.IsNotNull(actualBinding);
		}
		
		[Test]
		[Category("Integration")]
		public void GivenRegisteredBindingWithAFalseCondition_WhenTryGetBindingIsCalled_ThenItShouldReturnFalse()
		{
			/* GIVEN */
			this.container.Bind<ITestServiceA>()
				.To<TestServiceA>()
				.FromNew()
				.AsSingleton()
				.When(ctx => false);
			
			/* WHEN */
			var result = this.container.TryGetBinding<ITestServiceA>(out var actualBinding);
			
			/* THEN */
			Assert.IsFalse(result);
			Assert.IsNull(actualBinding);
		}
		
		[Test]
		[Category("Integration")]
		public void GivenRegisteredBindingWithAConditionThatThrows_WhenTryGetBindingIsCalled_ThenItShouldReturnFalse()
		{
			/* GIVEN */
			this.container.Bind<ITestServiceA>()
				.To<TestServiceA>()
				.FromNew()
				.AsSingleton()
				.When(ctx => throw new Exception());
			
			/* WHEN */
			var result = this.container.TryGetBinding<ITestServiceA>(out var actualBinding);
			
			/* THEN */
			Assert.IsFalse(result);
			Assert.IsNull(actualBinding);
		}

		public interface ITestServiceA
		{
		}
		
		public class TestServiceA : ITestServiceA
		{
		}
	}
}

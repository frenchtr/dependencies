﻿using NUnit.Framework;
using TravisRFrench.Dependencies.Runtime;
using TravisRFrench.Dependencies.Tests.Editor.Fakes;
using UnityEngine;

namespace TravisRFrench.Dependencies.Tests.Editor
{
    [TestFixture]
    [Category(Categories.Integration)]
    public class IntegrationTests
    {
        private IContext context;
        private IContainer container;
        
        [SetUp]
        public void SetUp()
        {
            this.context = new TestsContext();
            this.context.Initialize();
            
            this.container = this.context.Container;
        }
        
        [Test]
        public void GivenASingleDependencyRegisteredByItsType_WhenResolvedManually_ThenItShouldReturnTheCorrectDependency()
        {
            /* ARRANGE */
            var gameService = new GameService();
            this.container.Register<GameService>(gameService);
            
            /* ACT */
            var resolvedGameService = this.container.Resolve<GameService>();
            
            /* ASSERT */
            Assert.NotNull(resolvedGameService);
            Assert.AreSame(gameService, resolvedGameService);
        }
        
        [Test]
        public void GivenANewInjectable_WhenInjected_ThenItShouldHaveAllOfItsFieldDependenciesResolved()
        {
            /* ARRANGE */
            var gameService = new GameService();
            var player = new GameObject();
            var injectable = new Injectable();
            
            this.container.Register<GameService>(gameService);
            this.container.Register<GameObject>(player);
            
            /* ACT */
            this.container.Inject(injectable);
            
            /* ASSERT */
            Assert.NotNull(injectable.GameServiceFromInjectedField);
            Assert.NotNull(injectable.PlayerFromInjectedField);
            Assert.AreSame(gameService, injectable.GameServiceFromInjectedField);
            Assert.AreSame(player, injectable.PlayerFromInjectedField);
        }
        
        [Test]
        public void GivenANewInjectable_WhenInjected_ThenItShouldHaveAllOfItsPropertyDependenciesResolved()
        {
            /* ARRANGE */
            var gameService = new GameService();
            var player = new GameObject();
            var injectable = new Injectable();
            
            this.container.Register<GameService>(gameService);
            this.container.Register<GameObject>(player);
            
            /* ACT */
            this.container.Inject(injectable);
            
            /* ASSERT */
            Assert.NotNull(injectable.GameServiceFromInjectedProperty);
            Assert.NotNull(injectable.PlayerFromInjectedProperty);
            Assert.AreSame(gameService, injectable.GameServiceFromInjectedProperty);
            Assert.AreSame(player, injectable.PlayerFromInjectedProperty);
        }
        
        [Test]
        public void GivenANewInjectable_WhenInjected_ThenItShouldHaveAllOfItsMethodDependenciesResolved()
        {
            /* ARRANGE */
            var gameService = new GameService();
            var player = new GameObject();
            var injectable = new Injectable();
            
            this.container.Register<GameService>(gameService);
            this.container.Register<GameObject>(player);
            
            /* ACT */
            this.container.Inject(injectable);
            
            /* ASSERT */
            Assert.NotNull(injectable.GameServiceFromInjectedMethod);
            Assert.NotNull(injectable.PlayerFromInjectedMethod);
            Assert.AreSame(gameService, injectable.GameServiceFromInjectedMethod);
            Assert.AreSame(player, injectable.PlayerFromInjectedMethod);
        }
        
        [Test]
        public void GivenANewInjectable_WhenInjected_ThenItsConstructorDependenciesShouldBeResolved()
        {
            /* ARRANGE */
            var gameService = new GameService();
            var player = new GameObject();
            
            this.container.Register<GameService>(gameService);
            this.container.Register<GameObject>(player);
            
            /* ACT */
            var injectable = this.container.Resolve<Injectable>();
            
            /* ASSERT */
            Assert.NotNull(injectable.GameServiceFromConstructor);
            Assert.NotNull(injectable.PlayerFromConstructor);
            Assert.AreSame(gameService, injectable.GameServiceFromConstructor);
            Assert.AreSame(player, injectable.PlayerFromConstructor);
        }
        
        [Test]
        public void GivenANewInjectable_WhenInjected_ThenItsInjectedConstructorShouldBeCalled()
        {
            /* ARRANGE */
            var gameService = new GameService();
            
            this.container.Register<GameService>(gameService);
            
            /* ACT */
            var injectable = this.container.Resolve<Injectable>();
            
            /* ASSERT */
            Assert.NotNull(injectable.GameServiceFromConstructor);
            Assert.True(injectable.WasInjectedConstructorCalled);
        }
    }
}
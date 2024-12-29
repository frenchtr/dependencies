using NUnit.Framework;
using TravisRFrench.Dependencies.Runtime.Binding;
using TravisRFrench.Dependencies.Runtime.Containerization;
using TravisRFrench.Dependencies.Runtime.Contextualization;
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
        public void GivenDependencyRegisteredByType_WhenResolved_ThenItReturnsTheRegisteredInstance()
        {
            /* ARRANGE */
            var gameService = new GameService();
            this.container
                .Bind<GameService>()
                .ToSelf()
                .FromInstance(gameService);
            
            /* ACT */
            var resolvedGameService = this.container.Resolve<GameService>();
            
            /* ASSERT */
            Assert.NotNull(resolvedGameService);
            Assert.AreSame(gameService, resolvedGameService);
        }
        
        [Test]
        public void GivenDependencyRegisteredByInterface_WhenResolved_ThenItReturnsTheRegisteredInstance()
        {
            /* ARRANGE */
            var gameService = new GameService();
            this.container
                .Bind<IGameService>()
                .To<GameService>()
                .FromInstance(gameService);
            
            /* ACT */
            var resolvedGameService = this.container.Resolve<IGameService>();
            
            /* ASSERT */
            Assert.NotNull(resolvedGameService);
            Assert.AreSame(gameService, resolvedGameService);
        }
        
        [Test]
        public void GivenInjectableWithFieldDependencies_WhenInjected_ThenFieldsAreResolved()
        {
            /* ARRANGE */
            var gameService = new GameService();
            var player = new GameObject();
            var injectable = new Injectable();
            
            this.container
                .Bind<GameService>()
                .ToSelf()
                .FromInstance(gameService);

            this.container
                .Bind<GameObject>()
                .ToSelf()
                .FromInstance(player);
            
            /* ACT */
            this.container.Inject(injectable);
            
            /* ASSERT */
            Assert.NotNull(injectable.GameServiceFromInjectedField);
            Assert.NotNull(injectable.PlayerFromInjectedField);
            Assert.AreSame(gameService, injectable.GameServiceFromInjectedField);
            Assert.AreSame(player, injectable.PlayerFromInjectedField);
        }
        
        [Test]
        public void GivenInjectableWithPropertyDependencies_WhenInjected_ThenPropertiesAreResolved()
        {
            /* ARRANGE */
            var gameService = new GameService();
            var player = new GameObject();
            var injectable = new Injectable();
            
            this.container
                .Bind<GameService>()
                .ToSelf()
                .FromInstance(gameService);

            this.container
                .Bind<GameObject>()
                .ToSelf()
                .FromInstance(player);
            
            /* ACT */
            this.container.Inject(injectable);
            
            /* ASSERT */
            Assert.NotNull(injectable.GameServiceFromInjectedProperty);
            Assert.NotNull(injectable.PlayerFromInjectedProperty);
            Assert.AreSame(gameService, injectable.GameServiceFromInjectedProperty);
            Assert.AreSame(player, injectable.PlayerFromInjectedProperty);
        }
        
        [Test]
        public void GivenInjectableWithMethodDependencies_WhenInjected_ThenMethodsAreResolved()
        {
            /* ARRANGE */
            var gameService = new GameService();
            var player = new GameObject();
            var injectable = new Injectable();
            
            this.container
                .Bind<GameService>()
                .ToSelf()
                .FromInstance(gameService);

            this.container
                .Bind<GameObject>()
                .ToSelf()
                .FromInstance(player);
            
            /* ACT */
            this.container.Inject(injectable);
            
            /* ASSERT */
            Assert.NotNull(injectable.GameServiceFromInjectedMethod);
            Assert.NotNull(injectable.PlayerFromInjectedMethod);
            Assert.AreSame(gameService, injectable.GameServiceFromInjectedMethod);
            Assert.AreSame(player, injectable.PlayerFromInjectedMethod);
        }
    }
}

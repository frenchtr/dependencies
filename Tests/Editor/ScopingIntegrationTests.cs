using NUnit.Framework;
using TravisRFrench.Dependencies.Runtime.Binding;
using TravisRFrench.Dependencies.Runtime.Containerization;
using TravisRFrench.Dependencies.Tests.Editor.Fakes;

namespace TravisRFrench.Dependencies.Tests.Editor
{
    [TestFixture]
    [Category(Categories.Integration)]
    public class ScopingIntegrationTests
    {
        private IContainer container;

        [SetUp]
        public void SetUp()
        {
            this.container = new Container();
        }

        [Test]
        public void GivenSingletonDependencyRegisteredInParentScope_WhenResolvedInChildScope_ThenItUsesSameInstance()
        {
            /* ARRANGE */
            this.container
                .Bind<GameService>()
                .ToSelf()
                .AsSingleton();
            
            IGameService parentInstance = null;
            IGameService childInstance = null;
            
            /* ACT */
            using (var childScope = this.container.PushScope())
            {
                parentInstance = this.container.Resolve<GameService>();
                childInstance = childScope.Resolve<GameService>();
            }

            /* ASSERT */
            Assert.NotNull(parentInstance);
            Assert.NotNull(childInstance);
            Assert.AreSame(parentInstance, childInstance);
        }

        [Test]
        public void GivenTransientDependencyRegisteredInChildScope_WhenResolvedMultipleTimesInChildScope_ThenEachResolutionReturnsNewInstance()
        {
            /* ARRANGE */
            IGameService instance1 = null;
            IGameService instance2 = null;

            /* ACT */
            using (var childScope = this.container.PushScope())
            {
                childScope
                    .Bind<GameService>()
                    .ToSelf()
                    .AsTransient();
                
                instance1 = childScope.Resolve<GameService>();
                instance2 = childScope.Resolve<GameService>();
            }

            /* ASSERT */
            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void GivenScopedDependencyRegisteredInParent_WhenResolvedInChildScope_ThenItUsesParentScopeInstance()
        {
            /* ARRANGE */
            var gameService = new GameService();
            
            this.container
                .Bind<GameService>()
                .ToSelf()
                .FromInstance(gameService);
            
            IGameService parentInstance = null;
            IGameService childInstance = null;
            
            /* ACT */
            using (var childScope = this.container.PushScope())
            {
                parentInstance = this.container.Resolve<GameService>();
                childInstance = childScope.Resolve<GameService>();
            }

            /* ASSERT */
            Assert.NotNull(parentInstance);
            Assert.NotNull(childInstance);
            Assert.AreSame(parentInstance, childInstance);
        }
    }
}

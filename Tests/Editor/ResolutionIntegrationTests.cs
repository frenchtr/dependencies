using NUnit.Framework;
using TravisRFrench.Dependencies.Runtime.Binding;
using TravisRFrench.Dependencies.Runtime.Containerization;
using TravisRFrench.Dependencies.Runtime.Resolution;
using TravisRFrench.Dependencies.Tests.Editor.Fakes;

namespace TravisRFrench.Dependencies.Tests.Editor
{
    [TestFixture]
    [Category(Categories.Integration)]
    public class ResolutionIntegrationTests
    {
        private IContainer container;

        [SetUp]
        public void SetUp()
        {
            this.container = new Container();
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
        public void GivenDependencyRegisteredByInterface_WhenResolved_ThenItReturnsImplementationInstance()
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
        public void GivenUnregisteredType_WhenResolved_ThenItThrowsResolutionException()
        {
            /* ASSERT */
            Assert.Throws<ResolveException>(() => this.container.Resolve<GameService>());
        }

        [Test]
        public void GivenDependencyRegisteredByFactory_WhenResolved_ThenFactoryIsUsed()
        {
            /* ARRANGE */
            this.container
                .Bind<GameService>()
                .ToSelf()
                .FromFactory(() => new GameService());

            /* ACT */
            var resolvedGameService = this.container.Resolve<GameService>();

            /* ASSERT */
            Assert.NotNull(resolvedGameService);
        }
    }
}

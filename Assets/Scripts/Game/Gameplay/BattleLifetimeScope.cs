using Combat;
using Combat.Ai;
using Combat.Setup;
using Game.Gameplay.Config;
using Game.Gameplay.Controllers;
using Game.Gameplay.Presentation;
using Game.Gameplay.UI;
using UIKit.VContainer;
using VContainer;
using VContainer.Unity;

namespace Game.Gameplay
{
    public class BattleLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterScreen<BattleViewModel, BattleView>();
            builder.Register(CreateBattle, Lifetime.Scoped);
            builder.Register<BattleHud>(Lifetime.Scoped).As<IBattlePresentation>().AsSelf();
            builder.Register<PlayerController>(Lifetime.Scoped);
            builder.Register<AiDecisionMaker>(Lifetime.Scoped);
            builder.Register<AiController>(Lifetime.Scoped);
            builder.Register(CreateDriver, Lifetime.Scoped);
            builder.Register<BattleSession>(Lifetime.Scoped);
        }

        private Battle CreateBattle(IObjectResolver resolver)
        {
            var setup = resolver.Resolve<BattleConfig>().CreateSetup();
            return resolver.Resolve<BattleFactory>().Create(setup);
        }

        private BattleDriver CreateDriver(IObjectResolver resolver)
        {
            return new BattleDriver(
                resolver.Resolve<Battle>(),
                resolver.Resolve<IBattlePresentation>(),
                resolver.Resolve<PlayerController>(),
                resolver.Resolve<AiController>());
        }
    }
}

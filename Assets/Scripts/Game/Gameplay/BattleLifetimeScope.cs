using Combat;
using Combat.Ai;
using Combat.Rules;
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
            builder.Register<BattleLog>(Lifetime.Scoped);
            builder.Register<TurnGate>(Lifetime.Scoped);
            builder.Register<IBattlePresentation, LogPresenter>(Lifetime.Scoped);
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
            var random = resolver.Resolve<IRandom>();
            return new BattleDriver(
                resolver.Resolve<Battle>(),
                resolver.Resolve<IBattlePresentation>(),
                resolver.Resolve<TurnGate>(),
                new AiController(new AiDecisionMaker(random)),
                new AiController(new AiDecisionMaker(random)));
        }
    }
}

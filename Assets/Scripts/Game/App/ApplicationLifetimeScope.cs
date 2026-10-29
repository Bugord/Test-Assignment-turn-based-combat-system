using Combat.Rules;
using Combat.Setup;
using Game.App;
using Game.App.States;
using Game.Infrastructure;
using UniState;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class ApplicationLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterStateMachine<IAppStateMachine, AppStateMachine>(Lifetime.Scoped);
            builder.RegisterState<MenuState>();
            builder.RegisterState<BattleState>();
            builder.RegisterState<ClosingAppState>();
            builder.Register<IRandom, UnityRandom>(Lifetime.Singleton);
            builder.Register<BattleFactory>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}

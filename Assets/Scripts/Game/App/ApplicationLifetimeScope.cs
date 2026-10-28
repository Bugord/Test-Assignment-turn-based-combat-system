using Game.App;
using Game.App.States;
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
            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}

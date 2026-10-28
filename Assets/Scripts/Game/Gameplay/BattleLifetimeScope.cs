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
        }
    }
}

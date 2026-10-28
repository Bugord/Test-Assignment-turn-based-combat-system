using Game.Menu;
using UIKit.VContainer;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class MenuLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterScreen<MainMenuViewModel, MainMenuView>();
        }
    }
}

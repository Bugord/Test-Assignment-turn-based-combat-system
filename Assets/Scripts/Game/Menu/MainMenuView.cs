using UIKit;

namespace Game.Menu
{
    public class MainMenuView : View<MainMenuViewModel>
    {
        protected override void OnAttach()
        {
            BindCommand("battle-button", ViewModel.StartBattle);
            BindCommand("quit-button", ViewModel.Quit);
        }
    }
}

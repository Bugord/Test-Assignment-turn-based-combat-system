using UIKit;

namespace Game.Gameplay.UI
{
    public class PausePopupView : View<PausePopupViewModel>
    {
        protected override void OnAttach()
        {
            BindCommand("resume-button", ViewModel.Resume);
            BindCommand("restart-button", ViewModel.Restart);
            BindCommand("menu-button", ViewModel.BackToMenu);
        }
    }
}

using UIKit;

namespace Game.Gameplay
{
    public class BattleView : View<BattleViewModel>
    {
        protected override void OnAttach()
        {
            BindCommand("menu-button", ViewModel.BackToMenu);
        }
    }
}

using UIKit;
using UnityEngine.UIElements;

namespace Game.Gameplay.UI
{
    public class BattleResultPopupView : View<BattleResultPopupViewModel>
    {
        protected override void OnAttach()
        {
            Require<Label>("result-title").text = ViewModel.Title;
            BindCommand("restart-button", ViewModel.Restart);
            BindCommand("menu-button", ViewModel.BackToMenu);
        }
    }
}

using Combat.Units;
using UIKit;

namespace Game.Gameplay.UI
{
    [UIScreen(UILayer.Popup, Dismiss = DismissTriggers.None)]
    public class BattleResultPopupViewModel : ResultViewModel<BattleResult>, IPayloadReceiver<Team>
    {
        public string Title { get; private set; } = string.Empty;
        public Command Restart { get; }
        public Command BackToMenu { get; }

        public BattleResultPopupViewModel()
        {
            Restart = new Command(() => SetResult(BattleResult.Restart));
            BackToMenu = new Command(() => SetResult(BattleResult.BackToMenu));
        }

        public void SetPayload(Team winner)
        {
            Title = winner == Team.Player ? "Victory" : "Defeat";
        }
    }
}

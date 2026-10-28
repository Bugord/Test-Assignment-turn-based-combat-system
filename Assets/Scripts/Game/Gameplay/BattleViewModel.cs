using UIKit;

namespace Game.Gameplay
{
    public class BattleViewModel : ResultViewModel<BattleResult>
    {
        public Command BackToMenu { get; }

        public BattleViewModel()
        {
            BackToMenu = new Command(() => SetResult(BattleResult.BackToMenu));
        }
    }
}

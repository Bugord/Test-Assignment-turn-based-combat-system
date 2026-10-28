using UIKit;

namespace Game.Menu
{
    public class MainMenuViewModel : ResultViewModel<MainMenuResult>
    {
        public Command StartBattle { get; }
        public Command Quit { get; }

        public MainMenuViewModel()
        {
            StartBattle = new Command(() => SetResult(MainMenuResult.Battle));
            Quit = new Command(() => SetResult(MainMenuResult.Quit));
        }
    }
}

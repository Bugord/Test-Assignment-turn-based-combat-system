using UIKit;

namespace Game.Gameplay.UI
{
    [UIScreen(UILayer.Popup)]
    public class PausePopupViewModel : PopupViewModel<PauseResult>
    {
        public Command Resume { get; }
        public Command Restart { get; }
        public Command BackToMenu { get; }

        protected override PauseResult CancelResult => PauseResult.Resume;

        public PausePopupViewModel()
        {
            Resume = new Command(() => SetResult(PauseResult.Resume));
            Restart = new Command(() => SetResult(PauseResult.Restart));
            BackToMenu = new Command(() => SetResult(PauseResult.BackToMenu));
        }
    }
}

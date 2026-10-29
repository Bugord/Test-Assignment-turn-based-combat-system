using System;
using System.Collections.Generic;
using Game.Gameplay.Presentation;
using UIKit;
using Unity.Properties;

namespace Game.Gameplay.UI
{
    public class BattleViewModel : ResultViewModel<BattleResult>
    {
        private readonly BattleLog log;
        private readonly TurnGate turnGate;

        [CreateProperty]
        public IReadOnlyList<string> LogLines => log.Lines;

        public Command NextTurn { get; }
        public Command BackToMenu { get; }

        public BattleViewModel(BattleLog log, TurnGate turnGate)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.turnGate = turnGate ?? throw new ArgumentNullException(nameof(turnGate));

            NextTurn = new Command(turnGate.Open, () => turnGate.IsWaiting);
            BackToMenu = new Command(() => SetResult(BattleResult.BackToMenu));

            log.Changed += OnLogChanged;
            turnGate.Changed += NextTurn.RaiseCanExecuteChanged;
        }

        protected override void OnDispose()
        {
            log.Changed -= OnLogChanged;
            turnGate.Changed -= NextTurn.RaiseCanExecuteChanged;
        }

        private void OnLogChanged()
        {
            Notify(nameof(LogLines));
        }
    }
}

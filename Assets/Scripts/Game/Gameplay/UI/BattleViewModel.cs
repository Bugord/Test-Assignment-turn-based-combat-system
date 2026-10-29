using System;
using System.Collections.Generic;
using Combat.Abilities;
using Combat.Units;
using Game.Gameplay.Controllers;
using Game.Gameplay.Presentation;
using UIKit;
using Unity.Properties;

namespace Game.Gameplay.UI
{
    public class BattleViewModel : ResultViewModel<BattleResult>
    {
        private readonly BattleHud hud;
        private readonly PlayerController playerController;

        private Ability selectedAbility;

        [CreateProperty]
        public IReadOnlyList<string> LogLines => hud.Log;

        [CreateProperty]
        public string ActiveUnitText {
            get {
                if (hud.Winner != null) {
                    return $"{hud.Winner} won";
                }

                var unit = hud.ActiveUnit;
                return unit == null ? string.Empty : $"{unit}, HP {unit.Hp}/{unit.Stats.MaxHp}";
            }
        }

        [CreateProperty]
        public string Hint {
            get {
                if (playerController.CurrentTurnRequest != null) {
                    return selectedAbility == null ? "Choose an ability" : $"{selectedAbility}: choose a target";
                }

                return "Enemy turn";
            }
        }

        [CreateProperty]
        public IReadOnlyList<Ability> AbilityOptions {
            get {
                var request = playerController.CurrentTurnRequest;
                return request == null || selectedAbility != null ? Array.Empty<Ability>() : request.Abilities;
            }
        }

        [CreateProperty]
        public IReadOnlyList<Unit> TargetOptions {
            get {
                var request = playerController.CurrentTurnRequest;
                return request == null || selectedAbility == null ? Array.Empty<Unit>() : request.TargetsOf(selectedAbility);
            }
        }

        public Command CancelAbility { get; }
        public Command BackToMenu { get; }

        public BattleViewModel(BattleHud hud, PlayerController playerController)
        {
            this.hud = hud ?? throw new ArgumentNullException(nameof(hud));
            this.playerController = playerController ?? throw new ArgumentNullException(nameof(playerController));

            CancelAbility = new Command(() => SelectAbility(null), () => selectedAbility != null);
            BackToMenu = new Command(() => SetResult(BattleResult.BackToMenu));

            hud.LogChanged += OnLogChanged;
            hud.ActiveUnitChanged += OnActiveUnitChanged;
            playerController.CurrentTurnRequestChanged += OnCurrentTurnRequestChanged;
        }

        public void ChooseAbility(Ability ability)
        {
            var turnRequest = playerController.CurrentTurnRequest;
            if (turnRequest == null || selectedAbility != null) {
                return;
            }

            if (turnRequest.RequiresTarget(ability)) {
                SelectAbility(ability);
            }
            else {
                turnRequest.Submit(ability);
            }
        }

        public void ChooseTarget(Unit target)
        {
            var turnRequest = playerController.CurrentTurnRequest;
            if (turnRequest == null || selectedAbility == null) {
                return;
            }

            turnRequest.Submit(selectedAbility, target);
        }

        protected override void OnDispose()
        {
            hud.LogChanged -= OnLogChanged;
            hud.ActiveUnitChanged -= OnActiveUnitChanged;
            playerController.CurrentTurnRequestChanged -= OnCurrentTurnRequestChanged;
        }

        private void SelectAbility(Ability ability)
        {
            selectedAbility = ability;
            CancelAbility.RaiseCanExecuteChanged();
            Notify(nameof(AbilityOptions), nameof(TargetOptions), nameof(Hint));
        }
        
        private void OnLogChanged()
        {
            Notify(nameof(LogLines));
        }

        private void OnActiveUnitChanged()
        {
            Notify(nameof(ActiveUnitText));
        }

        private void OnCurrentTurnRequestChanged()
        {
            SelectAbility(null);
        }
    }
}

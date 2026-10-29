using System;
using System.Collections.Generic;
using System.Linq;
using Combat.Abilities;
using Combat.Units;
using Game.Gameplay.Config;
using Game.Gameplay.Controllers;
using Game.Gameplay.Presentation;
using UIKit;
using Unity.Properties;
using UnityEngine;

namespace Game.Gameplay.UI
{
    public class BattleViewModel : ResultViewModel<BattleResult>
    {
        private readonly BattleHud hud;
        private readonly PlayerController playerController;
        private readonly BattleConfig config;
        private readonly BattlePause battlePause;
        private readonly IScreenNavigator screenNavigator;

        private Ability selectedAbility;

        [CreateProperty]
        public IReadOnlyList<string> LogLines => hud.Log;

        [CreateProperty]
        public IReadOnlyList<Unit> TurnOrder {
            get {
                var active = hud.ActiveUnit;
                return active == null ? Array.Empty<Unit>() : hud.Queue.Prepend(active).ToList();
            }
        }

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
                if (hud.Winner != null) {
                    return string.Empty;
                }

                if (playerController.CurrentTurnRequest != null) {
                    return selectedAbility == null ? "Choose an ability" : $"{selectedAbility}: choose a target";
                }

                return hud.ActiveUnit?.Team == Team.Enemy ? "Enemy turn" : string.Empty;
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
        public AsyncCommand Pause { get; }

        public BattleViewModel(
            BattleHud hud,
            PlayerController playerController,
            BattleConfig config,
            BattlePause battlePause,
            IScreenNavigator screenNavigator)
        {
            this.hud = hud ?? throw new ArgumentNullException(nameof(hud));
            this.playerController = playerController ?? throw new ArgumentNullException(nameof(playerController));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.battlePause = battlePause ?? throw new ArgumentNullException(nameof(battlePause));
            this.screenNavigator = screenNavigator ?? throw new ArgumentNullException(nameof(screenNavigator));

            CancelAbility = new Command(() => SelectAbility(null), () => selectedAbility != null);
            Pause = new AsyncCommand(PauseAsync);

            hud.LogChanged += OnLogChanged;
            hud.TurnChanged += OnTurnChanged;
            playerController.CurrentTurnRequestChanged += OnCurrentTurnRequestChanged;
        }

        public Sprite PortraitOf(Unit unit) => config.UnitAt(unit.Team, unit.Slot).Sprite;

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
            hud.TurnChanged -= OnTurnChanged;
            playerController.CurrentTurnRequestChanged -= OnCurrentTurnRequestChanged;
        }

        private async Awaitable PauseAsync()
        {
            PauseResult result;
            battlePause.Pause();
            try {
                result = await screenNavigator.RunAsync<PausePopupViewModel, PauseResult>(LifetimeToken);
            }
            finally {
                battlePause.Resume();
            }

            switch (result) {
                case PauseResult.Restart:
                    SetResult(BattleResult.Restart);
                    break;
                case PauseResult.BackToMenu:
                    SetResult(BattleResult.BackToMenu);
                    break;
            }
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

        private void OnTurnChanged()
        {
            Notify(nameof(ActiveUnitText), nameof(Hint), nameof(TurnOrder));
        }

        private void OnCurrentTurnRequestChanged()
        {
            SelectAbility(null);
        }
    }
}

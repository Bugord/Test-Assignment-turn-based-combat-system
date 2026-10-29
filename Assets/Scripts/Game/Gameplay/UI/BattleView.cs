using System;
using System.Collections.Generic;
using Combat.Units;
using UIKit;
using UnityEngine.UIElements;

namespace Game.Gameplay.UI
{
    public class BattleView : View<BattleViewModel>
    {
        private readonly List<string> logLines = new List<string>();
        private ListView logList;
        private VisualElement turnQueue;
        private VisualElement abilityButtons;
        private VisualElement targetButtons;
        private VisualElement targetPanel;

        protected override void OnAttach()
        {
            logList = Require<ListView>("log-list");
            logList.makeItem = CreateLogItem;
            logList.bindItem = BindLogItem;
            logList.itemsSource = logLines;

            turnQueue = Require<VisualElement>("turn-queue");
            abilityButtons = Require<VisualElement>("ability-buttons");
            targetButtons = Require<VisualElement>("target-buttons");
            targetPanel = Require<VisualElement>("target-panel");

            BindText("active-unit-label", nameof(BattleViewModel.ActiveUnitText));
            BindText("hint-label", nameof(BattleViewModel.Hint));

            Observe(nameof(BattleViewModel.LogLines), SyncLog);
            Observe(nameof(BattleViewModel.TurnOrder), RebuildTurnQueue);
            Observe(nameof(BattleViewModel.AbilityOptions), RebuildAbilityButtons);
            Observe(nameof(BattleViewModel.TargetOptions), RebuildTargetButtons);
            SyncLog();
            RebuildTurnQueue();
            RebuildAbilityButtons();
            RebuildTargetButtons();

            BindCommand("cancel-ability-button", ViewModel.CancelAbility);
            BindCommand("menu-button", ViewModel.BackToMenu);
        }

        private VisualElement CreateLogItem()
        {
            var label = new Label();
            label.AddToClassList("battle-log-text");
            return label;
        }

        private void BindLogItem(VisualElement element, int index)
        {
            ((Label)element).text = logLines[index];
        }

        private void SyncLog()
        {
            var lines = ViewModel.LogLines;
            if (lines.Count == logLines.Count) {
                return;
            }

            for (var i = logLines.Count; i < lines.Count; i++) {
                logLines.Add(lines[i]);
            }

            logList.RefreshItems();
            logList.ScrollToItem(logLines.Count - 1);
        }

        private void RebuildTurnQueue()
        {
            turnQueue.Clear();
            var order = ViewModel.TurnOrder;
            for (var i = 0; i < order.Count; i++) {
                turnQueue.Add(CreateTurnQueueItem(order[i], i == 0));
            }
        }

        private VisualElement CreateTurnQueueItem(Unit unit, bool active)
        {
            var item = new VisualElement();
            item.AddToClassList("turn-queue-item");
            item.AddToClassList(unit.Team == Team.Player ? "turn-queue-item--player" : "turn-queue-item--enemy");
            item.EnableInClassList("turn-queue-item--active", active);

            var portrait = new VisualElement();
            portrait.AddToClassList("turn-queue-portrait");
            portrait.style.backgroundImage = new StyleBackground(ViewModel.PortraitOf(unit));
            item.Add(portrait);

            var name = new Label(unit.Name);
            name.AddToClassList("turn-queue-name");
            item.Add(name);

            return item;
        }

        private void RebuildAbilityButtons()
        {
            abilityButtons.Clear();
            foreach (var ability in ViewModel.AbilityOptions) {
                abilityButtons.Add(CreateChoiceButton(ability.Name, () => ViewModel.ChooseAbility(ability)));
            }
        }

        private void RebuildTargetButtons()
        {
            targetButtons.Clear();
            var targets = ViewModel.TargetOptions;
            foreach (var target in targets) {
                var text = $"{target}\nHP {target.Hp}/{target.Stats.MaxHp}";
                targetButtons.Add(CreateChoiceButton(text, () => ViewModel.ChooseTarget(target)));
            }

            targetPanel.style.display = targets.Count > 0 ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private Button CreateChoiceButton(string text, Action onClick)
        {
            var button = new Button(onClick) { text = text };
            button.AddToClassList("choice-button");
            return button;
        }
    }
}

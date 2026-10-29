using System.Collections.Generic;
using UIKit;
using UnityEngine.UIElements;

namespace Game.Gameplay.UI
{
    public class BattleView : View<BattleViewModel>
    {
        private readonly List<string> logLines = new List<string>();
        private ListView logList;

        protected override void OnAttach()
        {
            logList = Require<ListView>("log-list");
            logList.makeItem = CreateLogItem;
            logList.bindItem = BindLogItem;
            logList.itemsSource = logLines;

            Observe(nameof(BattleViewModel.LogLines), SyncLog);
            SyncLog();

            BindCommand("next-turn-button", ViewModel.NextTurn);
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
    }
}

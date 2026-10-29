using System;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Combat.Rules;
using Combat.Units;

namespace Game.Gameplay.Presentation
{
    public class BattleHud
    {
        private readonly List<string> log = new List<string>();
        private readonly Battle battle;

        public IReadOnlyList<string> Log => log;
        public Unit ActiveUnit { get; private set; }
        public Team? Winner { get; private set; }

        public event Action LogChanged;
        public event Action ActiveUnitChanged;

        public BattleHud(Battle battle)
        {
            this.battle = battle ?? throw new ArgumentNullException(nameof(battle));
        }

        public void SetActiveUnit(Unit actor)
        {
            ActiveUnit = actor;
            ActiveUnitChanged?.Invoke();
            AddLine($"Round {battle.Round}, {actor}'s turn");
        }

        public void AddAction(ActionResult result)
        {
            AddLine(Describe(result));
        }

        public void SetWinner(Team winner)
        {
            ActiveUnit = null;
            Winner = winner;
            ActiveUnitChanged?.Invoke();
            AddLine($"{winner} won in round {battle.Round}");
        }

        private void AddLine(string line)
        {
            log.Add(line);
            LogChanged?.Invoke();
        }

        private string Describe(ActionResult result)
        {
            if (result.Outcomes.Count == 0) {
                return $"{result.Actor} uses {result.Ability}";
            }

            var outcomes = string.Join(", ", result.Outcomes.Select(outcome => outcome.ToString()));
            return $"{result.Actor} uses {result.Ability}: {outcomes}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Combat;
using Combat.Rules;
using Combat.Units;
using Cysharp.Threading.Tasks;

namespace Game.Gameplay.Presentation
{
    public class BattleHud : IBattlePresentation
    {
        private readonly TimeSpan actionDelay = TimeSpan.FromSeconds(0.3);
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

        public UniTask ShowTurnStartAsync(Unit actor, CancellationToken token)
        {
            ActiveUnit = actor;
            ActiveUnitChanged?.Invoke();
            AddLine($"Round {battle.Round}, {actor}'s turn");
            return UniTask.CompletedTask;
        }

        public async UniTask PlayAsync(ActionResult result, CancellationToken token)
        {
            AddLine(Describe(result));
            await UniTask.Delay(actionDelay, cancellationToken: token);
        }

        public UniTask ShowBattleEndAsync(Team winner, CancellationToken token)
        {
            ActiveUnit = null;
            Winner = winner;
            ActiveUnitChanged?.Invoke();
            AddLine($"{winner} won in round {battle.Round}");
            return UniTask.CompletedTask;
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

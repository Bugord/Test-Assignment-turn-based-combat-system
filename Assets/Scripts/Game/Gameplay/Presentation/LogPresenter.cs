using System;
using System.Linq;
using System.Threading;
using Combat;
using Combat.Rules;
using Combat.Units;
using Cysharp.Threading.Tasks;

namespace Game.Gameplay.Presentation
{
    public class LogPresenter : IBattlePresentation
    {
        private readonly TimeSpan actionDelay = TimeSpan.FromSeconds(0.3);
        private readonly Battle battle;
        private readonly BattleLog log;

        public LogPresenter(Battle battle, BattleLog log)
        {
            this.battle = battle ?? throw new ArgumentNullException(nameof(battle));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public UniTask ShowTurnStartAsync(Unit actor, CancellationToken token)
        {
            log.Add($"Round {battle.Round}, {actor}'s turn");
            return UniTask.CompletedTask;
        }

        public async UniTask PlayAsync(ActionResult result, CancellationToken token)
        {
            log.Add(Describe(result));
            await UniTask.Delay(actionDelay, cancellationToken: token);
        }

        public UniTask ShowBattleEndAsync(Team winner, CancellationToken token)
        {
            log.Add($"{winner} won in round {battle.Round}");
            return UniTask.CompletedTask;
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

using System;
using System.Linq;
using System.Threading;
using Combat;
using Combat.Rules;
using Combat.Units;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Gameplay.Presentation
{
    public class LogPresenter : IBattlePresentation
    {
        private readonly TimeSpan actionDelay = TimeSpan.FromSeconds(0.3);
        private readonly TimeSpan endDelay = TimeSpan.FromSeconds(1);
        private readonly Battle battle;

        public LogPresenter(Battle battle)
        {
            this.battle = battle ?? throw new ArgumentNullException(nameof(battle));
        }

        public UniTask ShowTurnStartAsync(Unit actor, CancellationToken token)
        {
            Debug.Log($"Round {battle.Round}, {actor}'s turn");
            return UniTask.CompletedTask;
        }

        public async UniTask PlayAsync(ActionResult result, CancellationToken token)
        {
            Debug.Log(Describe(result));
            await UniTask.Delay(actionDelay, cancellationToken: token);
        }

        public async UniTask ShowBattleEndAsync(Team winner, CancellationToken token)
        {
            Debug.Log($"{winner} won in round {battle.Round}");
            await UniTask.Delay(endDelay, cancellationToken: token);
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

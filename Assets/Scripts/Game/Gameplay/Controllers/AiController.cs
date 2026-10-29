using System;
using System.Threading;
using Combat;
using Combat.Ai;
using Combat.Rules;
using Combat.Units;
using Cysharp.Threading.Tasks;

namespace Game.Gameplay.Controllers
{
    public class AiController : IUnitController
    {
        private readonly AiDecisionMaker decisionMaker;

        public AiController(AiDecisionMaker decisionMaker)
        {
            this.decisionMaker = decisionMaker ?? throw new ArgumentNullException(nameof(decisionMaker));
        }

        public UniTask<Decision> DecideAsync(Battle battle, Unit actor, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return UniTask.FromResult(decisionMaker.Decide(battle, actor));
        }
    }
}

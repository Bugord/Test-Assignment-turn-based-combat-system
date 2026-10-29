using System;
using System.Threading;
using Combat;
using Combat.Rules;
using Combat.Units;
using Cysharp.Threading.Tasks;

namespace Game.Gameplay.Controllers
{
    public class PlayerController : IUnitController
    {
        public TurnRequest CurrentTurnRequest { get; private set; }

        public event Action CurrentTurnRequestChanged;

        public async UniTask<Decision> DecideAsync(Battle battle, Unit actor, CancellationToken token)
        {
            if (CurrentTurnRequest != null) {
                throw new InvalidOperationException($"Still waiting for {CurrentTurnRequest.Actor}'s decision.");
            }

            CurrentTurnRequest = new TurnRequest(battle, actor);
            CurrentTurnRequestChanged?.Invoke();

            try {
                return await CurrentTurnRequest.Result.AttachExternalCancellation(token);
            }
            finally {
                CurrentTurnRequest = null;
                CurrentTurnRequestChanged?.Invoke();
            }
        }
    }
}

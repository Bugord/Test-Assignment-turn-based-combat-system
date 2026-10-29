using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Gameplay
{
    public class TurnGate
    {
        private UniTaskCompletionSource waiting;

        public bool IsWaiting => waiting != null;

        public event Action Changed;

        public async UniTask WaitAsync(CancellationToken token)
        {
            if (IsWaiting) {
                throw new InvalidOperationException("The gate is already waiting.");
            }

            waiting = new UniTaskCompletionSource();
            Changed?.Invoke();

            try {
                await waiting.Task.AttachExternalCancellation(token);
            }
            finally {
                waiting = null;
                Changed?.Invoke();
            }
        }

        public void Open()
        {
            waiting?.TrySetResult();
        }
    }
}

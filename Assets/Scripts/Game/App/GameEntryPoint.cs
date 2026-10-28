using System.Threading;
using Cysharp.Threading.Tasks;
using Game.App.States;
using VContainer.Unity;

namespace Game.App
{
    public class GameEntryPoint : IAsyncStartable
    {
        private readonly IAppStateMachine stateMachine;

        public GameEntryPoint(IAppStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await stateMachine.Execute<MenuState>(cancellation).SuppressCancellationThrow();
        }
    }
}

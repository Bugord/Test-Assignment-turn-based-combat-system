using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Gameplay;
using Game.Infrastructure;
using Game.Infrastructure.Scenes;
using UniState;
using VContainer;
using VContainer.Unity;

namespace Game.App.States
{
    public class BattleState : StateBase
    {
        private readonly LifetimeScope applicationScope;

        public BattleState(LifetimeScope applicationScope)
        {
            this.applicationScope = applicationScope;
        }

        public override async UniTask<StateTransitionInfo> Execute(CancellationToken token)
        {
            await RunBattleAsync(token);

            token.ThrowIfCancellationRequested();
            return Transition.GoTo<MenuState>();
        }

        private async UniTask RunBattleAsync(CancellationToken token)
        {
            await using var scene = await SceneLease.LoadAsync(SceneNames.Battle, token);
            using var scope = applicationScope.CreateChild<BattleLifetimeScope>(childScopeName: "Battle Scope");

            var session = scope.Container.Resolve<BattleSession>();
            await session.RunAsync(token);
        }
    }
}
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Gameplay;
using Game.Gameplay.Presentation;
using Game.Gameplay.UI;
using Game.Infrastructure;
using Game.Infrastructure.Scenes;
using UniState;
using UnityEngine;
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
            try {
                await RunBattleAsync(token);
            }
            catch (Exception exception) when (exception is not OperationCanceledException) {
                Debug.LogException(exception);
            }

            token.ThrowIfCancellationRequested();
            return Transition.GoTo<MenuState>();
        }

        private async UniTask RunBattleAsync(CancellationToken token)
        {
            await using var scene = await SceneLease.LoadAsync(SceneNames.Battle, token);
            var stage = scene.Find<BattleStage>();

            BattleResult result;
            do {
                result = await RunSessionAsync(stage, token);
            } while (result == BattleResult.Restart);
        }

        private async UniTask<BattleResult> RunSessionAsync(BattleStage stage, CancellationToken token)
        {
            using var scope = applicationScope.CreateChild<BattleLifetimeScope>(
                builder => builder.RegisterInstance(stage),
                "Battle Scope");

            var session = scope.Container.Resolve<BattleSession>();
            return await session.RunAsync(token);
        }
    }
}

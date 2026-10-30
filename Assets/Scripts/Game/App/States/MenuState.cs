using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Gameplay.Config;
using Game.Infrastructure;
using Game.Infrastructure.Scenes;
using Game.Menu;
using UIKit;
using UniState;
using VContainer;
using VContainer.Unity;

namespace Game.App.States
{
    public class MenuState : StateBase
    {
        private readonly LifetimeScope applicationScope;
        private readonly BattleConfig battleConfig;

        public MenuState(LifetimeScope applicationScope, BattleConfig battleConfig)
        {
            this.applicationScope = applicationScope;
            this.battleConfig = battleConfig;
        }

        public override async UniTask<StateTransitionInfo> Execute(CancellationToken token)
        {
            var result = await RunMenuAsync(token);
            token.ThrowIfCancellationRequested();

            return result switch {
                MainMenuResult.Battle => Transition.GoTo<BattleState, BattleLineup>(battleConfig.DefaultLineup),
                MainMenuResult.Quit => Transition.GoTo<ClosingAppState>(),
                _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
            };
        }

        private async UniTask<MainMenuResult> RunMenuAsync(CancellationToken token)
        {
            await using var scene = await SceneLease.LoadAsync(SceneNames.Menu, token);
            using var scope = applicationScope.CreateChild<MenuLifetimeScope>(childScopeName: "Menu Scope");

            var screenNavigator = scope.Container.Resolve<IScreenNavigator>();
            return await screenNavigator.RunAsync<MainMenuViewModel, MainMenuResult>(token);
        }
    }
}

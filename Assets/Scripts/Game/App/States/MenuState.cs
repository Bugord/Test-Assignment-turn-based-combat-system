using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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

        public MenuState(LifetimeScope applicationScope)
        {
            this.applicationScope = applicationScope;
        }

        public override async UniTask<StateTransitionInfo> Execute(CancellationToken token)
        {
            var result = await RunMenuAsync(token);
            token.ThrowIfCancellationRequested();

            return result switch {
                MainMenuResult.Battle => Transition.GoTo<BattleState>(),
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

using System;
using System.Threading;
using Combat.Units;
using Cysharp.Threading.Tasks;
using Game.Gameplay.UI;
using UIKit;

namespace Game.Gameplay
{
    public class BattleSession
    {
        private readonly BattleDriver driver;
        private readonly IScreenNavigator screenNavigator;

        public BattleSession(BattleDriver driver, IScreenNavigator screenNavigator)
        {
            this.driver = driver ?? throw new ArgumentNullException(nameof(driver));
            this.screenNavigator = screenNavigator ?? throw new ArgumentNullException(nameof(screenNavigator));
        }

        public async UniTask<BattleResult> RunAsync(CancellationToken token)
        {
            using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(token);
            var hud = screenNavigator.RunAsync<BattleViewModel, BattleResult>(cancellation.Token).AsUniTask().Preserve();
            var battle = PlayAsync(cancellation.Token).Preserve();

            try {
                var (winner, hudResult, battleResult) = await UniTask.WhenAny(hud, battle);
                return winner == 0 ? hudResult : battleResult;
            }
            finally {
                cancellation.Cancel();
                await hud.SuppressCancellationThrow();
                await battle.SuppressCancellationThrow();
            }
        }

        private async UniTask<BattleResult> PlayAsync(CancellationToken token)
        {
            var winner = await driver.RunAsync(token);
            return await screenNavigator.RunAsync<BattleResultPopupViewModel, Team, BattleResult>(winner, token);
        }
    }
}

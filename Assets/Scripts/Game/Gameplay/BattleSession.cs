using System;
using System.Threading;
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

        public async UniTask RunAsync(CancellationToken token)
        {
            using var battleCancellation = CancellationTokenSource.CreateLinkedTokenSource(token);
            using var screenCancellation = CancellationTokenSource.CreateLinkedTokenSource(token);
            var battle = RunBattleAsync(screenCancellation, battleCancellation.Token);

            try {
                await screenNavigator.RunAsync<BattleViewModel, BattleResult>(screenCancellation.Token);
            }
            finally {
                battleCancellation.Cancel();
                await battle;
            }
        }

        private async UniTask RunBattleAsync(CancellationTokenSource screenCancellation, CancellationToken token)
        {
            try {
                await driver.RunAsync(token);
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested) {
            }
            catch {
                screenCancellation.Cancel();
                throw;
            }
        }
    }
}

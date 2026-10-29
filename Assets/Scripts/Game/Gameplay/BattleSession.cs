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
            using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(token);
            var battle = driver.RunAsync(cancellation.Token).SuppressCancellationThrow();

            try {
                await screenNavigator.RunAsync<BattleViewModel, BattleResult>(token);
            }
            finally {
                cancellation.Cancel();
                await battle;
            }
        }
    }
}

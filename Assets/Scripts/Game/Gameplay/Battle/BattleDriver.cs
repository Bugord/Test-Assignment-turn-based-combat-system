using System;
using System.Threading;
using Combat;
using Combat.Units;
using Cysharp.Threading.Tasks;
using Game.Gameplay.Controllers;
using Game.Gameplay.Presentation;

namespace Game.Gameplay
{
    public class BattleDriver
    {
        private readonly Battle battle;
        private readonly IBattlePresentation presentation;
        private readonly IUnitController playerController;
        private readonly IUnitController enemyController;

        public BattleDriver(
            Battle battle,
            IBattlePresentation presentation,
            IUnitController playerController,
            IUnitController enemyController)
        {
            this.battle = battle ?? throw new ArgumentNullException(nameof(battle));
            this.presentation = presentation ?? throw new ArgumentNullException(nameof(presentation));
            this.playerController = playerController ?? throw new ArgumentNullException(nameof(playerController));
            this.enemyController = enemyController ?? throw new ArgumentNullException(nameof(enemyController));
        }

        public async UniTask<Team> RunAsync(CancellationToken token)
        {
            while (battle.Winner == null) {
                var actor = battle.ActiveUnit;
                await presentation.ShowTurnStartAsync(actor, token);

                var decision = await ControllerOf(actor).DecideAsync(battle, actor, token);
                var result = battle.Execute(decision);
                await presentation.PlayAsync(result, token);
            }

            var winner = battle.Winner.Value;
            await presentation.ShowBattleEndAsync(winner, token);
            return winner;
        }

        private IUnitController ControllerOf(Unit unit) => unit.Team == Team.Player ? playerController : enemyController;
    }
}

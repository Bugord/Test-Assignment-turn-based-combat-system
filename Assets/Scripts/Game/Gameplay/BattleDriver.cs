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
        private readonly TurnGate turnGate;
        private readonly IUnitController playerController;
        private readonly IUnitController enemyController;

        public BattleDriver(
            Battle battle,
            IBattlePresentation presentation,
            TurnGate turnGate,
            IUnitController playerController,
            IUnitController enemyController)
        {
            this.battle = battle ?? throw new ArgumentNullException(nameof(battle));
            this.presentation = presentation ?? throw new ArgumentNullException(nameof(presentation));
            this.turnGate = turnGate ?? throw new ArgumentNullException(nameof(turnGate));
            this.playerController = playerController ?? throw new ArgumentNullException(nameof(playerController));
            this.enemyController = enemyController ?? throw new ArgumentNullException(nameof(enemyController));
        }

        public async UniTask RunAsync(CancellationToken token)
        {
            while (battle.Winner == null) {
                var actor = battle.ActiveUnit;
                await presentation.ShowTurnStartAsync(actor, token);
                await turnGate.WaitAsync(token);

                var decision = await ControllerOf(actor).DecideAsync(battle, actor, token);
                var result = battle.Execute(decision);
                await presentation.PlayAsync(result, token);
            }

            await presentation.ShowBattleEndAsync(battle.Winner.Value, token);
        }

        private IUnitController ControllerOf(Unit unit) => unit.Team == Team.Player ? playerController : enemyController;
    }
}

using System.Threading;
using Combat.Rules;
using Combat.Units;
using Cysharp.Threading.Tasks;

namespace Game.Gameplay.Presentation
{
    public interface IBattlePresentation
    {
        UniTask ShowTurnStartAsync(Unit actor, CancellationToken token);
        UniTask PlayAsync(ActionResult result, CancellationToken token);
        UniTask ShowBattleEndAsync(Team winner, CancellationToken token);
    }
}

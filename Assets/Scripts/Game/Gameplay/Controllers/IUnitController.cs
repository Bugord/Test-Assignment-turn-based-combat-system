using System.Threading;
using Combat;
using Combat.Rules;
using Combat.Units;
using Cysharp.Threading.Tasks;

namespace Game.Gameplay.Controllers
{
    public interface IUnitController
    {
        UniTask<Decision> DecideAsync(Battle battle, Unit actor, CancellationToken token);
    }
}

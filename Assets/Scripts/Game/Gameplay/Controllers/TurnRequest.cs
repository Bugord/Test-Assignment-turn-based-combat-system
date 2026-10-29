using System;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Combat.Abilities;
using Combat.Rules;
using Combat.Units;
using Cysharp.Threading.Tasks;

namespace Game.Gameplay.Controllers
{
    public class TurnRequest
    {
        private readonly Dictionary<Ability, IReadOnlyList<Unit>> targets;
        private readonly UniTaskCompletionSource<Decision> completion = new UniTaskCompletionSource<Decision>();

        public Unit Actor { get; }
        public IReadOnlyList<Ability> Abilities { get; }
        public UniTask<Decision> Result => completion.Task;

        public TurnRequest(Battle battle, Unit actor)
        {
            if (battle == null) {
                throw new ArgumentNullException(nameof(battle));
            }

            Actor = actor ?? throw new ArgumentNullException(nameof(actor));
            Abilities = battle.AvailableAbilities(actor);
            targets = Abilities.ToDictionary(ability => ability, ability => battle.ValidTargets(actor, ability));
        }

        public bool RequiresTarget(Ability ability) => ability.TargetRule != TargetRule.None;

        public IReadOnlyList<Unit> TargetsOf(Ability ability)
        {
            return targets.TryGetValue(ability, out var valid) ? valid : Array.Empty<Unit>();
        }

        public void Submit(Ability ability, Unit target = null)
        {
            if (!targets.TryGetValue(ability, out var valid)) {
                throw new ArgumentException($"{Actor} can't use {ability} now.", nameof(ability));
            }

            if (RequiresTarget(ability) ? !valid.Contains(target) : target != null) {
                throw new ArgumentException($"{target} is not a valid target for {ability}.", nameof(target));
            }

            completion.TrySetResult(new Decision(Actor, ability, target));
        }
    }
}

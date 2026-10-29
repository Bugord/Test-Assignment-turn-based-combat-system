using System;
using System.Collections.Generic;
using System.Linq;
using Combat.Abilities;
using Combat.Rules;
using Combat.Units;

namespace Combat.Ai
{
    public class AiDecisionMaker
    {
        private readonly IRandom random;

        public AiDecisionMaker(IRandom random)
        {
            this.random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public Decision Decide(Battle battle, Unit actor)
        {
            foreach (var ability in battle.AvailableAbilities(actor)) {
                if (ability == Ability.Skip) {
                    continue;
                }

                var target = ChooseTarget(ability, battle.ValidTargets(actor, ability));
                if (target != null) {
                    return new Decision(actor, ability, target);
                }
            }

            return Decision.Skip(actor);
        }

        private Unit ChooseTarget(Ability ability, IReadOnlyList<Unit> targets)
        {
            if (targets.Count == 0) {
                return null;
            }

            if (ability.Effect == EffectType.Heal) {
                return targets
                    .Where(target => target.Stats.MaxHp - target.Hp >= ability.Power)
                    .OrderBy(target => target.Hp - target.Stats.MaxHp)
                    .FirstOrDefault();
            }

            var index = Math.Min((int)(random.Value() * targets.Count), targets.Count - 1);
            return targets[index];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Combat.Abilities;
using Combat.Units;

namespace Combat.Rules
{
    public class BattleRules
    {
        private readonly HitChanceSettings hitChance;
        private readonly IRandom random;

        public BattleRules(HitChanceSettings hitChance, IRandom random)
        {
            this.hitChance = hitChance ?? throw new ArgumentNullException(nameof(hitChance));
            this.random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public IReadOnlyList<Ability> AvailableAbilities(Unit actor, Group allies, Group opponents)
        {
            return actor.Abilities
                .Where(ability => ability.TargetRule == TargetRule.None || AvailableTargets(actor, ability, allies, opponents).Count > 0)
                .ToList();
        }

        public IReadOnlyList<Unit> AvailableTargets(Unit actor, Ability ability, Group allies, Group opponents)
        {
            if (ability.RequiresFrontline && allies.Frontline != actor) {
                return Array.Empty<Unit>();
            }

            switch (ability.TargetRule) {
                case TargetRule.None:
                    return Array.Empty<Unit>();
                case TargetRule.FrontlineEnemy:
                    var frontline = opponents.Frontline;
                    return frontline == null ? Array.Empty<Unit>() : new[] { frontline };
                case TargetRule.AnyEnemy:
                    return opponents.Alive.ToList();
                case TargetRule.AnyAlly:
                    return allies.Alive.ToList();
                default:
                    throw new ArgumentOutOfRangeException(nameof(ability), ability.TargetRule, null);
            }
        }

        public float HitChance(Unit actor, Ability ability, Unit target, Group allies)
        {
            var attack = actor.Stats.Attack;
            if (allies.Frontline == actor) {
                attack -= ability.FrontlinePenalty;
            }

            var chance = hitChance.Base + (attack - target.Stats.Defense) * hitChance.PerPoint;
            return Math.Clamp(chance, hitChance.Min, hitChance.Max);
        }

        public void Validate(Decision decision, Group allies, Group opponents)
        {
            var actor = decision.Actor;
            var ability = decision.Ability;

            if (!actor.IsAlive) {
                throw new ArgumentException($"{actor} is dead.", nameof(decision));
            }
            
            if (!AvailableAbilities(actor, allies, opponents).Contains(ability)) {
                throw new ArgumentException($"{actor} can't use {ability} now.", nameof(decision));
            }

            if (ability.TargetRule == TargetRule.None) {
                if (decision.Target != null) {
                    throw new ArgumentException($"{ability} takes no target.", nameof(decision));
                }
                return;
            }

            if (!AvailableTargets(actor, ability, allies, opponents).Contains(decision.Target)) {
                throw new ArgumentException($"{decision.Target} is not a valid target for {ability}.", nameof(decision));
            }
        }

        public IReadOnlyList<Unit> AffectedUnits(Ability ability, Unit target, Group allies, Group opponents)
        {
            var targetGroup = target.Team == allies.Team ? allies : opponents;

            return ability.Area switch {
                Area.Single => new[] { target },
                Area.Adjacent => targetGroup.Alive.Where(unit => Math.Abs(unit.Slot - target.Slot) <= 1).ToList(),
                Area.All => targetGroup.Alive.ToList(),
                _ => throw new ArgumentOutOfRangeException(nameof(ability), ability.Area, null)
            };
        }

        public ActionResult Apply(Decision decision, Group allies, Group opponents)
        {
            if (decision.Target == null) {
                return new ActionResult(decision.Actor, decision.Ability, Array.Empty<TargetOutcome>());
            }

            var outcomes = AffectedUnits(decision.Ability, decision.Target, allies, opponents)
                .Select(target => Resolve(decision.Actor, decision.Ability, target, allies))
                .ToList();
            
            return new ActionResult(decision.Actor, decision.Ability, outcomes);
        }

        private TargetOutcome Resolve(Unit actor, Ability ability, Unit target, Group allies)
        {
            var hpBefore = target.Hp;
            var hit = !ability.RequiresHitRoll || random.Value() < HitChance(actor, ability, target, allies);

            if (!hit) {
                return new TargetOutcome(target, false, hpBefore, target.Hp);
            }
            
            switch (ability.Effect) {
                case EffectType.Damage:
                    target.TakeDamage(ability.Power);
                    break;
                case EffectType.Heal:
                    target.Heal(ability.Power);
                    break;
                case EffectType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new TargetOutcome(target, true, hpBefore, target.Hp);
        }
    }
}

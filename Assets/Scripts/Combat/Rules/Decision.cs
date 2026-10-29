using System;
using Combat.Abilities;
using Combat.Units;

namespace Combat.Rules
{
    public class Decision
    {
        public Unit Actor { get; }
        public Ability Ability { get; }
        public Unit Target { get; }

        public Decision(Unit actor, Ability ability, Unit target = null)
        {
            Actor = actor ?? throw new ArgumentNullException(nameof(actor));
            Ability = ability ?? throw new ArgumentNullException(nameof(ability));
            Target = target;
        }

        public static Decision Skip(Unit actor) => new Decision(actor, Ability.Skip);

        public override string ToString() => Target == null ? $"{Actor}: {Ability}" : $"{Actor}: {Ability} -> {Target}";
    }
}

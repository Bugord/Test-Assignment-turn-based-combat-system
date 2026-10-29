using System.Collections.Generic;
using Combat.Abilities;
using Combat.Units;

namespace Combat.Rules
{
    public class ActionResult
    {
        public Unit Actor { get; }
        public Ability Ability { get; }
        public IReadOnlyList<TargetOutcome> Outcomes { get; }

        public ActionResult(Unit actor, Ability ability, IReadOnlyList<TargetOutcome> outcomes)
        {
            Actor = actor;
            Ability = ability;
            Outcomes = outcomes;
        }
    }
}

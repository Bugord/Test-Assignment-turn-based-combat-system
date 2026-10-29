using System;
using System.Linq;
using Combat.Rules;
using Combat.Units;

namespace Combat.Setup
{
    public class BattleFactory
    {
        private readonly IRandom random;

        public BattleFactory(IRandom random)
        {
            this.random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public Battle Create(BattleSetup setup)
        {
            var player = CreateGroup(Team.Player, setup.Player);
            var enemy = CreateGroup(Team.Enemy, setup.Enemy);
            return new Battle(player, enemy, new BattleRules(setup.HitChance, random));
        }

        private Group CreateGroup(Team team, GroupDefinition definition)
        {
            var units = definition.Slots
                .Select((unit, slot) => (unit, slot))
                .Where(entry => entry.unit != null)
                .Select(entry => CreateUnit(team, entry.slot, entry.unit));

            return new Group(team, definition.SlotCount, units);
        }

        private Unit CreateUnit(Team team, int slot, UnitDefinition definition)
        {
            var id = $"{team}-{slot}".ToLowerInvariant();
            return new Unit(id, definition.Name, team, slot, definition.Stats, definition.Abilities);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Combat.Abilities;
using Combat.Units;

namespace Combat.Setup
{
    public class UnitDefinition
    {
        public string Name { get; }
        public Stats Stats { get; }
        public IReadOnlyList<Ability> Abilities { get; }

        public UnitDefinition(string name, Stats stats, IEnumerable<Ability> abilities)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Stats = stats;
            Abilities = abilities?.ToList() ?? throw new ArgumentNullException(nameof(abilities));
        }
    }
}

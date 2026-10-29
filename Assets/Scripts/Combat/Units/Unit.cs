using System;
using System.Collections.Generic;
using System.Linq;
using Combat.Abilities;

namespace Combat.Units
{
    public class Unit
    {
        public string Id { get; }
        public string Name { get; }
        public Team Team { get; }
        public int Slot { get; }
        public Stats Stats { get; }
        public IReadOnlyList<Ability> Abilities { get; }

        public int Hp { get; private set; }
        public bool IsAlive => Hp > 0;

        public Unit(string id, string name, Team team, int slot, Stats stats, IEnumerable<Ability> abilities)
        {
            if (slot < 0) {
                throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }

            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Team = team;
            Slot = slot;
            Stats = stats;
            Abilities = abilities
                .Where(ability => ability != Ability.Skip)
                .Append(Ability.Skip)
                .ToList();
            
            Hp = stats.MaxHp;
        }

        public void TakeDamage(int amount)
        {
            Hp = Math.Max(0, Hp - amount);
        }

        public void Heal(int amount)
        {
            Hp = Math.Min(Stats.MaxHp, Hp + amount);
        }

        public override string ToString() => $"{Name} ({Team} {Slot})";
    }
}

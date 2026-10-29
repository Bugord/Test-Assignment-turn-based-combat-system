using System;
using System.Collections.Generic;
using System.Linq;

namespace Combat.Units
{
    public class Group
    {
        public Team Team { get; }
        public int SlotCount { get; }
        public IReadOnlyList<Unit> Units { get; }

        public Unit Frontline => Units.FirstOrDefault(unit => unit.IsAlive);
        public bool HasAlive => Frontline != null;
        public IEnumerable<Unit> Alive => Units.Where(unit => unit.IsAlive);

        public Group(Team team, int slotCount, IEnumerable<Unit> units)
        {
            if (slotCount <= 0) {
                throw new ArgumentOutOfRangeException(nameof(slotCount), slotCount, "A group needs at least one slot.");
            }

            var list = units.OrderBy(unit => unit.Slot).ToList();

            if (list.Count == 0) {
                throw new ArgumentException("A group needs at least one unit.", nameof(units));
            }
            if (list.Any(unit => unit.Team != team)) {
                throw new ArgumentException($"Every unit of the {team} group must be on that side.", nameof(units));
            }
            if (list.Any(unit => unit.Slot >= slotCount)) {
                throw new ArgumentException($"The {team} group has only {slotCount} slots.", nameof(units));
            }
            if (list.Select(unit => unit.Slot).Distinct().Count() != list.Count) {
                throw new ArgumentException("Two units share a slot.", nameof(units));
            }

            Team = team;
            SlotCount = slotCount;
            Units = list;
        }
    }
}

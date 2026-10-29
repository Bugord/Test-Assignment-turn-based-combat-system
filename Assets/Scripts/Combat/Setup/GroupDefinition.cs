using System;
using System.Collections.Generic;
using System.Linq;

namespace Combat.Setup
{
    public class GroupDefinition
    {
        public int SlotCount { get; }
        public IReadOnlyList<UnitDefinition> Slots { get; }

        public GroupDefinition(int slotCount, IEnumerable<UnitDefinition> slots)
        {
            var list = slots?.ToList() ?? throw new ArgumentNullException(nameof(slots));

            if (list.Count > slotCount) {
                throw new ArgumentException($"{list.Count} units don't fit into {slotCount} slots.", nameof(slots));
            }

            SlotCount = slotCount;
            Slots = list;
        }
    }
}

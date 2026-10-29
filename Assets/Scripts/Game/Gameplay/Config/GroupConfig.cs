using System;
using System.Collections.Generic;
using System.Linq;
using Combat.Setup;
using UnityEngine;

namespace Game.Gameplay.Config
{
    [Serializable]
    public class GroupConfig
    {
        [SerializeField]
        [Min(1)]
        private int slotCount = 4;

        [SerializeField]
        private List<UnitConfig> slots = new List<UnitConfig>();

        public UnitConfig UnitAt(int slot) => slots[slot];

        public GroupDefinition CreateDefinition()
        {
            var units = slots.Select(unit => unit == null ? null : unit.CreateDefinition());
            return new GroupDefinition(slotCount, units);
        }
    }
}
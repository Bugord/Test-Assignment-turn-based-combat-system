using System;
using System.Collections.Generic;
using System.Linq;
using Combat.Abilities;
using Combat.Setup;
using UnityEngine;

namespace Game.Gameplay.Config
{
    [Serializable]
    public class GroupConfig
    {
        [SerializeField, Min(1)]
        private int slotCount = 4;

        [SerializeField]
        private List<UnitConfig> slots = new List<UnitConfig>();

        public GroupDefinition CreateDefinition(Func<AbilityConfig, Ability> getAbility)
        {
            var units = slots.Select(unit => unit == null ? null : unit.CreateDefinition(getAbility));
            return new GroupDefinition(slotCount, units);
        }
    }
}

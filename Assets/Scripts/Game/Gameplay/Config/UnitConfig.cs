using System.Collections.Generic;
using System.Linq;
using Combat.Setup;
using Combat.Units;
using UnityEngine;

namespace Game.Gameplay.Config
{
    [CreateAssetMenu(menuName = "Battle/Unit", fileName = "Unit")]
    public class UnitConfig : ScriptableObject
    {
        [SerializeField]
        private string displayName;

        [SerializeField][Min(1)]
        private int maxHp = 20;

        [SerializeField]
        private int speed;

        [SerializeField]
        private int attack;

        [SerializeField]
        private int defense;

        [SerializeField]
        private List<AbilityConfig> abilities = new List<AbilityConfig>();

        public UnitDefinition CreateDefinition()
        {
            var stats = new Stats(maxHp, speed, attack, defense);
            return new UnitDefinition(displayName, stats, abilities.Select(ability => ability.CreateAbility()));
        }
    }
}

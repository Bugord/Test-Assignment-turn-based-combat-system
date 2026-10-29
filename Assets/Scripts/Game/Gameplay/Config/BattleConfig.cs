using Combat.Rules;
using Combat.Setup;
using Combat.Units;
using UnityEngine;

namespace Game.Gameplay.Config
{
    [CreateAssetMenu(menuName = "Battle/Battle", fileName = "Battle")]
    public class BattleConfig : ScriptableObject
    {
        [SerializeField]
        private GroupConfig player = new GroupConfig();

        [SerializeField]
        private GroupConfig enemy = new GroupConfig();

        [Header("Hit chance")]
        [SerializeField]
        [Range(0f, 1f)]
        private float baseHitChance = 0.75f;

        [SerializeField]
        [Min(0f)]
        private float hitChancePerPoint = 0.05f;

        [SerializeField]
        [Range(0f, 1f)]
        private float minHitChance = 0.1f;

        [SerializeField]
        [Range(0f, 1f)]
        private float maxHitChance = 0.95f;

        public UnitConfig UnitAt(Team team, int slot) => (team == Team.Player ? player : enemy).UnitAt(slot);

        public BattleSetup CreateSetup()
        {
            var hitChance = new HitChanceSettings(baseHitChance, hitChancePerPoint, minHitChance, maxHitChance);
            return new BattleSetup(player.CreateDefinition(), enemy.CreateDefinition(), hitChance);
        }
    }
}
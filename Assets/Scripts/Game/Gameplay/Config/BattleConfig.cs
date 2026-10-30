using Combat.Rules;
using Combat.Setup;
using UnityEngine;

namespace Game.Gameplay.Config
{
    [CreateAssetMenu(menuName = "Battle/Battle", fileName = "Battle")]
    public class BattleConfig : ScriptableObject
    {
        [SerializeField]
        private BattleLineup defaultLineup = new BattleLineup();

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

        public BattleLineup DefaultLineup => defaultLineup;

        public BattleSetup CreateSetup(BattleLineup lineup)
        {
            var hitChance = new HitChanceSettings(baseHitChance, hitChancePerPoint, minHitChance, maxHitChance);
            return new BattleSetup(lineup.Player.CreateDefinition(), lineup.Enemy.CreateDefinition(), hitChance);
        }
    }
}
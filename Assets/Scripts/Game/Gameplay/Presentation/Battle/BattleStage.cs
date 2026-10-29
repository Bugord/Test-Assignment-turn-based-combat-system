using System.Collections.Generic;
using Combat.Units;
using UnityEngine;

namespace Game.Gameplay.Presentation
{
    public class BattleStage : MonoBehaviour
    {
        [SerializeField]
        private UnitView unitViewPrefab;

        [SerializeField]
        private List<Transform> playerSlots = new List<Transform>();

        [SerializeField]
        private List<Transform> enemySlots = new List<Transform>();

        public UnitView Spawn(Team team, int slot)
        {
            var anchor = (team == Team.Player ? playerSlots : enemySlots)[slot];
            return Instantiate(unitViewPrefab, anchor.position, Quaternion.identity, anchor);
        }
    }
}

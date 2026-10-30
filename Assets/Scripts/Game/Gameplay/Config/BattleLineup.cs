using System;
using Combat.Units;
using UnityEngine;

namespace Game.Gameplay.Config
{
    [Serializable]
    public class BattleLineup
    {
        [SerializeField]
        private GroupConfig player;

        [SerializeField]
        private GroupConfig enemy;

        public GroupConfig Player => player;
        public GroupConfig Enemy => enemy;

        public BattleLineup()
        {
        }

        public BattleLineup(GroupConfig player, GroupConfig enemy)
        {
            this.player = player ?? throw new ArgumentNullException(nameof(player));
            this.enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public UnitConfig UnitConfigAt(Team team, int slot) => (team == Team.Player ? player : enemy).UnitAt(slot);
    }
}

using System;
using Combat.Rules;

namespace Combat.Setup
{
    public class BattleSetup
    {
        public GroupDefinition Player { get; }
        public GroupDefinition Enemy { get; }
        public HitChanceConfig HitChance { get; }

        public BattleSetup(GroupDefinition player, GroupDefinition enemy, HitChanceConfig hitChance)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
            Enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
            HitChance = hitChance ?? throw new ArgumentNullException(nameof(hitChance));
        }
    }
}

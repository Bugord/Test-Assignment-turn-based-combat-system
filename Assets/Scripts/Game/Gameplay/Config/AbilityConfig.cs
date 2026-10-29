using Combat.Abilities;
using UnityEngine;

namespace Game.Gameplay.Config
{
    [CreateAssetMenu(menuName = "Battle/Ability", fileName = "Ability")]
    public class AbilityConfig : ScriptableObject
    {
        [SerializeField]
        private string displayName;

        [SerializeField]
        private TargetRule targetRule;

        [SerializeField]
        private Area area;

        [SerializeField]
        private EffectType effect;

        [SerializeField]
        [Min(0)]
        private int power;

        [SerializeField]
        private bool requiresHitRoll;

        [SerializeField]
        private bool requiresFrontline;

        [SerializeField]
        [Min(0)]
        private int frontlinePenalty;

        public Ability CreateAbility()
        {
            return new Ability(name, displayName, targetRule, effect, power, area,
                requiresHitRoll: requiresHitRoll,
                requiresFrontline: requiresFrontline,
                frontlinePenalty: frontlinePenalty);
        }
    }
}
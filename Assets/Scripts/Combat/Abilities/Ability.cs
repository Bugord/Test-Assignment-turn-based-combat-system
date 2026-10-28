using System;

namespace Combat.Abilities
{
    public class Ability
    {
        public static readonly Ability Skip = new Ability("skip", "Skip", TargetRule.None, EffectType.None, 0);

        public string Id { get; }
        public string Name { get; }
        public TargetRule TargetRule { get; }
        public Area Area { get; }
        public EffectType Effect { get; }
        public int Power { get; }
        public bool RequiresHitRoll { get; }
        public bool RequiresFrontline { get; }
        public int FrontlinePenalty { get; }

        public Ability(
            string id,
            string name,
            TargetRule targetRule,
            EffectType effect,
            int power,
            Area area = Area.Single,
            bool requiresHitRoll = false,
            bool requiresFrontline = false,
            int frontlinePenalty = 0)
        {
            if (power < 0) {
                throw new ArgumentOutOfRangeException(nameof(power), power, "Power can't be negative.");
            }
            
            if ((targetRule == TargetRule.None) != (effect == EffectType.None)) {
                throw new ArgumentException($"{name}: an ability has a target exactly when it has an effect.");
            }

            if (targetRule == TargetRule.None && area != Area.Single) {
                throw new ArgumentException($"{name}: an ability without a target can't have an area.");
            }

            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            TargetRule = targetRule;
            Area = area;
            Effect = effect;
            Power = power;
            RequiresHitRoll = requiresHitRoll;
            RequiresFrontline = requiresFrontline;
            FrontlinePenalty = frontlinePenalty;
        }

        public override string ToString() => Name;
    }
}

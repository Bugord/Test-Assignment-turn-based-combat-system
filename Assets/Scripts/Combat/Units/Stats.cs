using System;

namespace Combat.Units
{
    public readonly struct Stats
    {
        public int MaxHp { get; }
        public int Speed { get; }
        public int Attack { get; }
        public int Defense { get; }

        public Stats(int maxHp, int speed, int attack, int defense)
        {
            if (maxHp <= 0) {
                throw new ArgumentOutOfRangeException(nameof(maxHp), maxHp, "Max HP must be positive.");
            }

            MaxHp = maxHp;
            Speed = speed;
            Attack = attack;
            Defense = defense;
        }
    }
}

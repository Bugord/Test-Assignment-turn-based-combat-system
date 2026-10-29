using System;

namespace Combat.Rules
{
    public class HitChanceSettings
    {
        public float Base { get; }
        public float PerPoint { get; }
        public float Min { get; }
        public float Max { get; }

        public HitChanceSettings(float @base, float perPoint, float min, float max)
        {
            if (min < 0f || max > 1f || min > max) {
                throw new ArgumentException($"Hit chance bounds must satisfy 0 <= min <= max <= 1, got {min}..{max}.");
            }
            if (perPoint < 0f) {
                throw new ArgumentOutOfRangeException(nameof(perPoint), perPoint, "Per point bonus can't be negative.");
            }

            Base = @base;
            PerPoint = perPoint;
            Min = min;
            Max = max;
        }
    }
}

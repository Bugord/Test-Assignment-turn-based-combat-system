using Combat.Rules;
using UnityEngine;

namespace Game.Infrastructure
{
    public class UnityRandom : IRandom
    {
        public float Value() => Random.value;
    }
}

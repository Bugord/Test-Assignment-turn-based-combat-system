using Combat.Rules;
using UnityEngine;

namespace Game.Gameplay
{
    public class UnityRandom : IRandom
    {
        public float Value() => Random.value;
    }
}

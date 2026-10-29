using Combat.Units;

namespace Combat.Rules
{
    public class TargetOutcome
    {
        public Unit Target { get; }
        public bool Hit { get; }
        public int HpBefore { get; }
        public int HpAfter { get; }
        public bool Died => HpBefore > 0 && HpAfter == 0;

        public TargetOutcome(Unit target, bool hit, int hpBefore, int hpAfter)
        {
            Target = target;
            Hit = hit;
            HpBefore = hpBefore;
            HpAfter = hpAfter;
        }

        public override string ToString()
        {
            if (!Hit) {
                return $"{Target}: miss";
            }
            
            return Died ? $"{Target}: {HpBefore} -> {HpAfter}, died" : $"{Target}: {HpBefore} -> {HpAfter}";
        }
    }
}

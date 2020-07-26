/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

namespace Rakrae.Unity.Health
{
    public class PeriodicDamageEffect
    {
        public PeriodicDamageEffect(float ticksPerSecond, float damagePerTick, bool firstTickImmediately = false, float durationSeconds = 0)
        {
            TicksPerSecond = ticksPerSecond;
            DamagePerTick = damagePerTick;
            FirstTickImmediately = firstTickImmediately;
            DurationSeconds = durationSeconds;
        }

        public float TicksPerSecond { get; }
        public float DamagePerTick { get; }
        public bool FirstTickImmediately { get; }
        public float DurationSeconds { get; }
    }
}

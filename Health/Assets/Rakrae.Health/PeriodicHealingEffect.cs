/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

namespace Rakrae.Unity.Health
{
    public class PeriodicHealingEffect
    {
        public PeriodicHealingEffect(float ticksPerSecond, float healingPerTick, bool firstTickImmediately = false, float durationSeconds = 0)
        {
            TicksPerSecond = ticksPerSecond;
            HealingPerTick = healingPerTick;
            FirstTickImmediately = firstTickImmediately;
            DurationSeconds = durationSeconds;
        }

        public float TicksPerSecond { get; }
        public float HealingPerTick { get; }
        public bool FirstTickImmediately { get; }
        public float DurationSeconds { get; }
    }
}

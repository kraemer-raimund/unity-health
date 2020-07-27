/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

namespace Rakrae.Unity.Health.Events
{
    public class ShieldChangedEventArgs
    {
        public ShieldChangedEventArgs(float maxCharge, float currentCharge)
        {
            MaxCharge = maxCharge;
            CurrentCharge = currentCharge;
        }

        public float MaxCharge { get; }
        public float CurrentCharge { get; }
    }
}

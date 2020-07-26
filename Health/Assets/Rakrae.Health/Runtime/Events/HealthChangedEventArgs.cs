/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

namespace Rakrae.Unity.Health.Events
{
    public class HealthChangedEventArgs
    {
        public HealthChangedEventArgs(float newHealth)
        {
            NewHealth = newHealth;
        }

        public float NewHealth { get; }
    }
}

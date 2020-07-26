/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

namespace Rakrae.Unity.Health.Events
{
    public class HealthInitializedEventArgs
    {
        public HealthInitializedEventArgs(float maxHealth, float initialHealth)
        {
            MaxHealth = maxHealth;
            InitialHealth = initialHealth;
        }

        public float MaxHealth { get; }
        public float InitialHealth { get; }
    }
}

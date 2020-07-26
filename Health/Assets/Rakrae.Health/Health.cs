/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

using System;

namespace Rakrae.Unity.Health
{
    public class Health
    {
        public Health(float maxHealth, float initialHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = initialHealth;
        }

        public bool IsAlive => CurrentHealth > 0;
        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; private set; }

        public void Reduce(float amount)
        {
            CurrentHealth = Math.Max(CurrentHealth - amount, 0.0f);
        }

        public void Heal(float amount)
        {
            CurrentHealth = Math.Max(CurrentHealth + amount, MaxHealth);
        }

        public void Restore()
        {
            CurrentHealth = MaxHealth;
        }
    }
}

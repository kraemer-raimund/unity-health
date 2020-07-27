/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

using Rakrae.Unity.Health.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Rakrae.Unity.Health.Behaviours
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] private float _maxHealth = 100.0f;
        [SerializeField] private float _initialHealth = 100.0f;

        [SerializeField] private HealthInitializedEvent _healthInitialized = null;
        [SerializeField] private HealthChangedEvent _healthChanged = null;
        [SerializeField] private UnityEvent _damageTaken = null;

        private Health _health;
        private PeriodicHealthEffects _periodicHealthEffects;

        private void Start()
        {
            InitializeHealth();
            InitializePeriodicEffects();
        }

        public void ApplyDamage(float amount)
        {
            _health.Reduce(amount);
            _healthChanged.Invoke(new HealthChangedEventArgs(_health.CurrentHealth));
            _damageTaken.Invoke();
        }

        public void AddHealingEffect(PeriodicHealingEffect healingEffect)
        {
            _periodicHealthEffects.AddHealingEffect(healingEffect);
        }

        public void AddDamageEffect(PeriodicDamageEffect damageEffect)
        {
            _periodicHealthEffects.AddDamageEffect(damageEffect);
        }

        public void ToggleSelfHealing()
        {
            _periodicHealthEffects.ToggleSelfHealing();
        }

        public void ClearPeriodicEffects()
        {
            _periodicHealthEffects.ClearAll();
        }

        private void InitializeHealth()
        {
            _health = new Health(_maxHealth, _initialHealth);
            _healthInitialized.Invoke(new HealthInitializedEventArgs(_maxHealth, _initialHealth));
        }

        private void InitializePeriodicEffects()
        {
            _periodicHealthEffects = new PeriodicHealthEffects(this);

            _periodicHealthEffects.Healed += (s, amount) =>
            {
                _health.Heal(amount);
                _healthChanged.Invoke(new HealthChangedEventArgs(_health.CurrentHealth));
            };

            _periodicHealthEffects.Damaged += (s, amount) =>
            {
                _health.Reduce(amount);
                _healthChanged.Invoke(new HealthChangedEventArgs(_health.CurrentHealth));
            };
        }
    }
}

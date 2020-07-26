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
            _health = new Health(_maxHealth, _initialHealth);
            _periodicHealthEffects = new PeriodicHealthEffects(this, _health);

            _periodicHealthEffects.HealthChanged += (s, e) => _healthChanged.Invoke(e);

            _healthInitialized.Invoke(new HealthInitializedEventArgs(_maxHealth, _initialHealth));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _health.Reduce(10);

                _healthChanged.Invoke(new HealthChangedEventArgs(_health.CurrentHealth));
                _damageTaken.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                AddHealingEffect(new PeriodicHealingEffect(2, 5, true, 4));
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                AddDamageEffect(new PeriodicDamageEffect(1, 4, false, 10));
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                _periodicHealthEffects.ToggleSelfHealing();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                _periodicHealthEffects.ClearAll();
            }
        }

        private void AddHealingEffect(PeriodicHealingEffect healingEffect)
        {
            _periodicHealthEffects.AddHealingEffect(healingEffect);
        }

        private void AddDamageEffect(PeriodicDamageEffect damageEffect)
        {
            _periodicHealthEffects.AddDamageEffect(damageEffect);
        }


    }
}

/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

using System.Collections;
using System.Collections.Generic;
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

        private readonly ICollection<Coroutine> _periodicHealingEffects = new HashSet<Coroutine>();
        private readonly ICollection<Coroutine> _periodicDamageEffects = new HashSet<Coroutine>();
        private Health _health;
        private Coroutine _selfHealing;

        private void Start()
        {
            _health = new Health(_maxHealth, _initialHealth);

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
                if (_selfHealing != null)
                {
                    StopCoroutine(_selfHealing);
                    _selfHealing = null;
                }
                else
                {
                    _selfHealing = StartCoroutine(PeriodicHealingEffectCoroutine(new PeriodicHealingEffect(0.5f, 2)));
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                foreach (Coroutine coroutine in _periodicHealingEffects)
                {
                    StopCoroutine(coroutine);
                }

                _periodicHealingEffects.Clear();

                foreach (Coroutine coroutine in _periodicDamageEffects)
                {
                    StopCoroutine(coroutine);
                }

                _periodicDamageEffects.Clear();

                if (_selfHealing != null)
                {
                    StopCoroutine(_selfHealing);
                    _selfHealing = null;
                }
            }
        }

        private void AddHealingEffect(PeriodicHealingEffect healingEffect)
        {
            Coroutine coroutine = StartCoroutine(PeriodicHealingEffectCoroutine(healingEffect));
            _periodicHealingEffects.Add(coroutine);

            if (healingEffect.DurationSeconds > 0)
            {
                StartCoroutine(StopAndRemoveCoroutineAfterDuration(coroutine, _periodicHealingEffects, healingEffect.DurationSeconds));
            }
        }

        private void AddDamageEffect(PeriodicDamageEffect damageEffect)
        {
            Coroutine coroutine = StartCoroutine(PeriodicDamageEffectCoroutine(damageEffect));
            _periodicDamageEffects.Add(coroutine);

            if (damageEffect.DurationSeconds > 0)
            {
                StartCoroutine(StopAndRemoveCoroutineAfterDuration(coroutine, _periodicDamageEffects, damageEffect.DurationSeconds));
            }
        }

        private IEnumerator PeriodicHealingEffectCoroutine(PeriodicHealingEffect healingEffect)
        {
            if (healingEffect.FirstTickImmediately)
            {
                _health.Heal(healingEffect.HealingPerTick);
                _healthChanged.Invoke(new HealthChangedEventArgs(_health.CurrentHealth));
            }

            while (true)
            {
                yield return new WaitForSeconds(1 / healingEffect.TicksPerSecond);

                _health.Heal(healingEffect.HealingPerTick);
                _healthChanged.Invoke(new HealthChangedEventArgs(_health.CurrentHealth));
            }
        }

        private IEnumerator PeriodicDamageEffectCoroutine(PeriodicDamageEffect damageEffect)
        {
            if (damageEffect.FirstTickImmediately)
            {
                _health.Reduce(damageEffect.DamagePerTick);
                _healthChanged.Invoke(new HealthChangedEventArgs(_health.CurrentHealth));
            }

            while (true)
            {
                yield return new WaitForSeconds(1 / damageEffect.TicksPerSecond);

                _health.Reduce(damageEffect.DamagePerTick);
                _healthChanged.Invoke(new HealthChangedEventArgs(_health.CurrentHealth));
            }
        }

        private IEnumerator StopAndRemoveCoroutineAfterDuration(Coroutine coroutine, ICollection<Coroutine> coroutines, float duration)
        {
            yield return new WaitForSeconds(duration);

            _ = coroutines.Remove(coroutine);

            StopCoroutine(coroutine);
        }
    }
}

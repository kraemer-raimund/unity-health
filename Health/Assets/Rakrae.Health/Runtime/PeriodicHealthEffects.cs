/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Rakrae.Unity.Health.Events;
using UnityEngine;

namespace Rakrae.Unity.Health
{
    public class PeriodicHealthEffects
    {
        private readonly MonoBehaviour _coroutineOwner;
        private readonly Health _health;
        private readonly ICollection<Coroutine> _periodicHealingEffects = new HashSet<Coroutine>();
        private readonly ICollection<Coroutine> _periodicDamageEffects = new HashSet<Coroutine>();
        private Coroutine _selfHealing;

        public PeriodicHealthEffects(MonoBehaviour coroutineOwner, Health health)
        {
            _coroutineOwner = coroutineOwner;
            _health = health;
        }

        public event EventHandler<HealthChangedEventArgs> HealthChanged;

        public void AddHealingEffect(PeriodicHealingEffect healingEffect)
        {
            Coroutine coroutine = _coroutineOwner.StartCoroutine(PeriodicHealingEffectCoroutine(healingEffect));
            _periodicHealingEffects.Add(coroutine);

            if (healingEffect.DurationSeconds > 0)
            {
                _coroutineOwner.StartCoroutine(StopAndRemoveCoroutineAfterDuration(coroutine, _periodicHealingEffects, healingEffect.DurationSeconds));
            }
        }

        public void AddDamageEffect(PeriodicDamageEffect damageEffect)
        {
            Coroutine coroutine = _coroutineOwner.StartCoroutine(PeriodicDamageEffectCoroutine(damageEffect));
            _periodicDamageEffects.Add(coroutine);

            if (damageEffect.DurationSeconds > 0)
            {
                _coroutineOwner.StartCoroutine(StopAndRemoveCoroutineAfterDuration(coroutine, _periodicDamageEffects, damageEffect.DurationSeconds));
            }
        }

        public void ToggleSelfHealing()
        {
            if (_selfHealing != null)
            {
                _coroutineOwner.StopCoroutine(_selfHealing);
                _selfHealing = null;
            }
            else
            {
                _selfHealing = _coroutineOwner.StartCoroutine(PeriodicHealingEffectCoroutine(new PeriodicHealingEffect(0.5f, 2)));
            }
        }

        public void ClearAll()
        {
            foreach (Coroutine coroutine in _periodicHealingEffects)
            {
                _coroutineOwner.StopCoroutine(coroutine);
            }

            _periodicHealingEffects.Clear();

            foreach (Coroutine coroutine in _periodicDamageEffects)
            {
                _coroutineOwner.StopCoroutine(coroutine);
            }

            _periodicDamageEffects.Clear();

            if (_selfHealing != null)
            {
                _coroutineOwner.StopCoroutine(_selfHealing);
                _selfHealing = null;
            }
        }

        private IEnumerator PeriodicHealingEffectCoroutine(PeriodicHealingEffect healingEffect)
        {
            if (healingEffect.FirstTickImmediately)
            {
                _health.Heal(healingEffect.HealingPerTick);
                HealthChanged?.Invoke(this, new HealthChangedEventArgs(_health.CurrentHealth));
            }

            while (true)
            {
                yield return new WaitForSeconds(1 / healingEffect.TicksPerSecond);

                _health.Heal(healingEffect.HealingPerTick);
                HealthChanged?.Invoke(this, new HealthChangedEventArgs(_health.CurrentHealth));
            }
        }

        private IEnumerator PeriodicDamageEffectCoroutine(PeriodicDamageEffect damageEffect)
        {
            if (damageEffect.FirstTickImmediately)
            {
                _health.Reduce(damageEffect.DamagePerTick);
                HealthChanged?.Invoke(this, new HealthChangedEventArgs(_health.CurrentHealth));
            }

            while (true)
            {
                yield return new WaitForSeconds(1 / damageEffect.TicksPerSecond);

                _health.Reduce(damageEffect.DamagePerTick);
                HealthChanged?.Invoke(this, new HealthChangedEventArgs(_health.CurrentHealth));
            }
        }

        private IEnumerator StopAndRemoveCoroutineAfterDuration(Coroutine coroutine, ICollection<Coroutine> coroutines, float duration)
        {
            yield return new WaitForSeconds(duration);

            _ = coroutines.Remove(coroutine);

            _coroutineOwner.StopCoroutine(coroutine);
        }
    }
}

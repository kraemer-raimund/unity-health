/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rakrae.Unity.Health
{
    public class PeriodicHealthEffects
    {
        private readonly MonoBehaviour _coroutineOwner;
        private readonly PeriodicHealingEffect _selfHealingEffect = new PeriodicHealingEffect(0.5f, 2);

        private readonly ICollection<Coroutine> _periodicHealingEffects = new HashSet<Coroutine>();
        private readonly ICollection<Coroutine> _periodicDamageEffects = new HashSet<Coroutine>();
        private Coroutine _selfHealing;

        public PeriodicHealthEffects(MonoBehaviour coroutineOwner)
        {
            _coroutineOwner = coroutineOwner;
        }

        public event EventHandler<float> Damaged;
        public event EventHandler<float> Healed;

        public void AddHealingEffect(PeriodicHealingEffect healingEffect)
        {
            void applyHealing()
            {
                Healed?.Invoke(this, healingEffect.HealingPerTick);
            }

            Coroutine coroutine = _coroutineOwner.StartCoroutine(
                PeriodicEffectCoroutine(
                    healingEffect.TicksPerSecond,
                    healingEffect.FirstTickImmediately,
                    applyHealing
                )
            );

            AddPeriodicEffect(coroutine, _periodicHealingEffects, healingEffect.DurationSeconds);
        }

        public void AddDamageEffect(PeriodicDamageEffect damageEffect)
        {
            void applyDamage()
            {
                Damaged?.Invoke(this, damageEffect.DamagePerTick);
            }

            Coroutine coroutine = _coroutineOwner.StartCoroutine(
                PeriodicEffectCoroutine(
                    damageEffect.TicksPerSecond,
                    damageEffect.FirstTickImmediately,
                    applyDamage
                )
            );

            AddPeriodicEffect(coroutine, _periodicDamageEffects, damageEffect.DurationSeconds);
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
                void applyHealing()
                {
                    Healed?.Invoke(this, _selfHealingEffect.HealingPerTick);
                }

                _selfHealing = _coroutineOwner.StartCoroutine(
                    PeriodicEffectCoroutine(
                        _selfHealingEffect.TicksPerSecond,
                        _selfHealingEffect.FirstTickImmediately,
                        applyHealing
                    )
                );
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

        private void AddPeriodicEffect(Coroutine coroutine, ICollection<Coroutine> coroutines, float durationSeconds)
        {
            coroutines.Add(coroutine);

            if (durationSeconds > 0)
            {
                _coroutineOwner.StartCoroutine(
                    StopAndRemoveCoroutineAfterDuration(
                        coroutine,
                        coroutines,
                        durationSeconds
                    )
                );
            }
        }

        private IEnumerator PeriodicEffectCoroutine(float ticksPerSecond, bool firstTickImmediately, Action applyEffect)
        {
            bool isFirstTick = true;

            while (true)
            {
                if (firstTickImmediately || !isFirstTick)
                {
                    applyEffect?.Invoke();
                }

                yield return new WaitForSeconds(1 / ticksPerSecond);

                isFirstTick = false;
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

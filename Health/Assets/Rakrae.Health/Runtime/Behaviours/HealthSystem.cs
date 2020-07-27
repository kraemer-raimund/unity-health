/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

using System.Collections;
using Rakrae.Unity.Health.SerializedFieldGroups;
using UnityEngine;

namespace Rakrae.Unity.Health.Behaviours
{
    public class HealthSystem : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private HealthSettings _healthSettings = null;

        [Header("Shield")]
        [SerializeField] private ShieldSettings _shieldSettings = null;

        private Health _health;
        private PeriodicHealthEffects _periodicHealthEffects;
        private Shield _shield;
        private Coroutine _shieldRecharge;

        private void Start()
        {
            InitializeHealth();
            InitializePeriodicEffects();
            InitializeShield();
        }

        public void ApplyDamage(float amount)
        {
            if (amount <= 0 || !_health.IsAlive)
            {
                return;
            }

            StopRecharging();

            if (_shieldSettings.EnableAutoRecharge)
            {
                StartRechargingAfterDelay();
            }

            bool didBlockDamage = _shield.TryBlockDamage(amount, out float damageAfterShield);

            if (didBlockDamage)
            {
                OnShieldChargeChanged();

                if (_shield.CurrentCharge > 0)
                {
                    OnShieldDamaged();
                }
                else
                {
                    OnShieldDestroyed();
                }
            }

            if (damageAfterShield > 0)
            {
                _health.Reduce(damageAfterShield);
                OnHealthChanged();

                if (_health.IsAlive)
                {
                    OnDamageTaken();
                }
                else
                {
                    OnDied();
                }
            }
        }

        public void Heal(float amount)
        {
            if (amount <= 0 || !_health.IsAlive)
            {
                return;
            }

            _health.Heal(amount);
            OnHealthChanged();

            if (_health.CurrentHealth < _health.MaxHealth)
            {
                OnPartlyHealed();
            }
            else
            {
                OnFullyHealed();
            }
        }

        public void RechargeShield(float amount)
        {
            if (amount <= 0 || !_health.IsAlive)
            {
                return;
            }

            _shield.Recharge(amount);

            if (_shield.CurrentCharge < _shield.MaxCharge)
            {
                OnShieldPartlyRecharged();
            }
            else
            {
                OnShieldFullyRecharged();
            }
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

        private void OnHealthChanged()
        {
            _healthSettings.Events.HealthChanged.Invoke(_health.CurrentHealth);
        }

        private void OnMaxHealthChanged()
        {
            _healthSettings.Events.MaxHealthChanged.Invoke(_health.MaxHealth);
        }

        private void OnDamageTaken()
        {
            _healthSettings.Events.DamageTaken.Invoke();
        }

        private void OnDied()
        {
            StopRecharging();
            _healthSettings.Events.Died.Invoke();
        }

        private void OnPartlyHealed()
        {
            _healthSettings.Events.PartlyHealed.Invoke();
        }

        private void OnFullyHealed()
        {
            _healthSettings.Events.FullyHealed.Invoke();
        }

        private void OnShieldChargeChanged()
        {
            _shieldSettings.Events.ShieldChargeChanged.Invoke(_shield.CurrentCharge);
        }

        private void OnMaxShieldChargeChanged()
        {
            _shieldSettings.Events.MaxShieldChargeChanged.Invoke(_shield.MaxCharge);
        }

        private void OnShieldDamaged()
        {
            _shieldSettings.Events.ShieldDamaged.Invoke();
        }

        private void OnShieldDestroyed()
        {
            _shieldSettings.Events.ShieldDestroyed.Invoke();
        }

        private void OnShieldPartlyRecharged()
        {
            _shieldSettings.Events.ShieldPartlyRecharged.Invoke();
        }

        private void OnShieldFullyRecharged()
        {
            _shieldSettings.Events.ShieldFullyRecharged.Invoke();
        }

        private void InitializeHealth()
        {
            _health = new Health(_healthSettings.MaxHealth, _healthSettings.InitialHealth);
            OnMaxHealthChanged();
            OnHealthChanged();
        }

        private void InitializePeriodicEffects()
        {
            _periodicHealthEffects = new PeriodicHealthEffects(this);

            _periodicHealthEffects.Healed += (s, amount) =>
            {
                Heal(amount);
                OnDamageTaken();
            };

            _periodicHealthEffects.Damaged += (s, amount) =>
            {
                ApplyDamage(amount);
                OnDamageTaken();
            };
        }

        private void InitializeShield()
        {
            _shield = new Shield(
                _shieldSettings.MaxShieldCharge,
                _shieldSettings.InitialShieldCharge,
                _shieldSettings.ShieldBlockPercentage
            );
            OnMaxShieldChargeChanged();
            OnShieldChargeChanged();
        }

        private void StopRecharging()
        {
            if (_shieldRecharge != null)
            {
                StopCoroutine(_shieldRecharge);
                _shieldRecharge = null;
            }
        }

        private void StartRechargingAfterDelay()
        {
            _shieldRecharge = StartCoroutine(ShieldRechargeCoroutine(_shieldSettings));
        }

        private IEnumerator ShieldRechargeCoroutine(ShieldSettings shieldSettings)
        {
            yield return new WaitForSeconds(shieldSettings.BeginRechargeSecondsAfterDamageTaken);

            while (_shield.CurrentCharge < _shield.MaxCharge)
            {
                _shield.Recharge(shieldSettings.RechargeAmountPerTick);
                OnShieldChargeChanged();
                OnShieldPartlyRecharged();
                yield return new WaitForSeconds(1.0f / shieldSettings.RechargeTicksPerSecond);
            }

            OnShieldFullyRecharged();
        }
    }
}

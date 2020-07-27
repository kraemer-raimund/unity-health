/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

using System.Collections;
using Rakrae.Unity.Health.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Rakrae.Unity.Health.Behaviours
{
    public class HealthSystem : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float _maxHealth = 100.0f;
        [SerializeField] private float _initialHealth = 100.0f;

        [Header("Shield")]
        [SerializeField] private float _maxShieldCharge = 100.0f;
        [SerializeField] private float _initialShieldCharge = 100.0f;
        [SerializeField] private float _shieldBlockPercentage = 80.0f;
        [SerializeField] private bool _enableAutoRecharge = true;
        [SerializeField] private float _beginRechargeSecondsAfterDamageTaken = 5.0f;
        [SerializeField] private float _rechargeTicksPerSecond = 2.0f;
        [SerializeField] private float _rechargeAmountPerTick = 5.0f;

        [Header("Basic Health Events")]
        [SerializeField] private HealthChangedEvent _healthChanged = null;

        [Header("Advanced Health Events")]
        [SerializeField] private UnityEvent _damageTaken = null;
        [SerializeField] private UnityEvent _died = null;
        [SerializeField] private UnityEvent _partlyHealed = null;
        [SerializeField] private UnityEvent _fullyHealed = null;

        [Header("Basic Shield Events")]
        [SerializeField] private ShieldChangedEvent _shieldChanged = null;

        [Header("Advanced Shield Events")]
        [SerializeField] private UnityEvent _shieldDamaged = null;
        [SerializeField] private UnityEvent _shieldDestroyed = null;
        [SerializeField] private UnityEvent _shieldPartlyRecharged = null;
        [SerializeField] private UnityEvent _shieldFullyRecharged = null;

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

            if (_enableAutoRecharge)
            {
                StartRechargingAfterDelay();
            }

            bool didBlockDamage = _shield.TryBlockDamage(amount, out float damageAfterShield);

            if (didBlockDamage)
            {
                OnShieldChanged();
                
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
            _healthChanged.Invoke(new HealthChangedEventArgs(_health.MaxHealth, _health.CurrentHealth));
        }

        private void OnDamageTaken()
        {
            _damageTaken.Invoke();
        }

        private void OnDied()
        {
            StopRecharging();
            _died.Invoke();
        }

        private void OnPartlyHealed()
        {
            _partlyHealed.Invoke();
        }

        private void OnFullyHealed()
        {
            _fullyHealed.Invoke();
        }

        private void OnShieldChanged()
        {
            _shieldChanged.Invoke(new ShieldChangedEventArgs(_shield.MaxCharge, _shield.CurrentCharge));
        }

        private void OnShieldDamaged()
        {
            _shieldDamaged.Invoke();
        }

        private void OnShieldDestroyed()
        {
            _shieldDestroyed.Invoke();
        }

        private void OnShieldPartlyRecharged()
        {
            _shieldPartlyRecharged.Invoke();
        }

        private void OnShieldFullyRecharged()
        {
            _shieldFullyRecharged.Invoke();
        }

        private void InitializeHealth()
        {
            _health = new Health(_maxHealth, _initialHealth);
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
            _shield = new Shield(_maxShieldCharge, _initialShieldCharge, _shieldBlockPercentage);
            OnShieldChanged();
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
            _shieldRecharge = StartCoroutine(
                ShieldRechargeCoroutine(
                    _beginRechargeSecondsAfterDamageTaken,
                    _rechargeTicksPerSecond,
                    _rechargeAmountPerTick
                )
            );
        }

        private IEnumerator ShieldRechargeCoroutine(float delay, float ticksPerSecond, float amountPerTick)
        {
            yield return new WaitForSeconds(delay);

            while (_shield.CurrentCharge < _shield.MaxCharge)
            {
                _shield.Recharge(amountPerTick);
                OnShieldChanged();
                OnShieldPartlyRecharged();
                yield return new WaitForSeconds(1.0f / ticksPerSecond);
            }

            OnShieldFullyRecharged();
        }
    }
}

/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Rakrae.Unity.Health.SerializedFieldGroups
{
    [Serializable]
    public class ShieldSettings
    {
        [SerializeField] private float _maxShieldCharge = 100.0f;
        [SerializeField] private float _initialShieldCharge = 100.0f;
        [SerializeField] private float _shieldBlockPercentage = 100.0f;
        [SerializeField] private bool _enableAutoRecharge = true;
        [SerializeField] private float _beginRechargeSecondsAfterDamageTaken = 5.0f;
        [SerializeField] private float _rechargeTicksPerSecond = 2.0f;
        [SerializeField] private float _rechargeAmountPerTick = 5.0f;
        [SerializeField] private ShieldEvents _events = null;

        public float MaxShieldCharge => _maxShieldCharge;
        public float InitialShieldCharge => _initialShieldCharge;
        public float ShieldBlockPercentage => _shieldBlockPercentage;
        public bool EnableAutoRecharge => _enableAutoRecharge;
        public float BeginRechargeSecondsAfterDamageTaken => _beginRechargeSecondsAfterDamageTaken;
        public float RechargeTicksPerSecond => _rechargeTicksPerSecond;
        public float RechargeAmountPerTick => _rechargeAmountPerTick;

        public ShieldEvents Events => _events;

        [Serializable]
        public class ShieldEvents
        {
            [SerializeField] private UnityEvent<float> _shieldChargeChanged = null;
            [SerializeField] private UnityEvent<float> _maxShieldChargeChanged = null;
            [SerializeField] private UnityEvent _shieldDamaged = null;
            [SerializeField] private UnityEvent _shieldDestroyed = null;
            [SerializeField] private UnityEvent _shieldPartlyRecharged = null;
            [SerializeField] private UnityEvent _shieldFullyRecharged = null;

            public UnityEvent<float> ShieldChargeChanged => _shieldChargeChanged;
            public UnityEvent<float> MaxShieldChargeChanged => _maxShieldChargeChanged;
            public UnityEvent ShieldDamaged => _shieldDamaged;
            public UnityEvent ShieldDestroyed => _shieldDestroyed;
            public UnityEvent ShieldPartlyRecharged => _shieldPartlyRecharged;
            public UnityEvent ShieldFullyRecharged => _shieldFullyRecharged;
        }
    }
}

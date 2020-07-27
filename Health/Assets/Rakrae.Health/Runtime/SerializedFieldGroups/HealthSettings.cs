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
    public class HealthSettings
    {
        [SerializeField] private float _maxHealth = 100.0f;
        [SerializeField] private float _initialHealth = 100.0f;
        [SerializeField] private HealthEvents _events = null;

        public float MaxHealth => _maxHealth;
        public float InitialHealth => _initialHealth;
        public HealthEvents Events => _events;

        [Serializable]
        public class HealthEvents
        {
            [SerializeField] private UnityEvent<float> _healthChanged = null;
            [SerializeField] private UnityEvent<float> _maxHealthChanged = null;
            [SerializeField] private UnityEvent _damageTaken = null;
            [SerializeField] private UnityEvent _died = null;
            [SerializeField] private UnityEvent _partlyHealed = null;
            [SerializeField] private UnityEvent _fullyHealed = null;

            public UnityEvent<float> HealthChanged => _healthChanged;
            public UnityEvent<float> MaxHealthChanged => _maxHealthChanged;
            public UnityEvent DamageTaken => _damageTaken;
            public UnityEvent Died => _died;
            public UnityEvent PartlyHealed => _partlyHealed;
            public UnityEvent FullyHealed => _fullyHealed;
        }
    }
}

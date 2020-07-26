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
        }
    }
}

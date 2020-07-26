/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

using Rakrae.Unity.Health.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Rakrae.Unity.Health.Example
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] Slider _healthBar = null;
        [SerializeField] Text _currentHealth = null;
        [SerializeField] Text _maxHealth = null;

        public void OnHealthInitialized(HealthInitializedEventArgs healthInitializedEventArgs)
        {
            if (_healthBar)
            {
                _healthBar.minValue = 0;
                _healthBar.maxValue = healthInitializedEventArgs.MaxHealth;
                _healthBar.value = healthInitializedEventArgs.InitialHealth;
            }

            if (_currentHealth)
            {
                _currentHealth.text = Mathf.RoundToInt(healthInitializedEventArgs.InitialHealth).ToString();
            }

            if (_maxHealth)
            {
                _maxHealth.text = Mathf.RoundToInt(healthInitializedEventArgs.MaxHealth).ToString();
            }
        }

        public void OnHealthChanged(HealthChangedEventArgs healthChangedEventArgs)
        {
            if (_healthBar)
            {
                _healthBar.value = healthChangedEventArgs.NewHealth;
            }

            if (_currentHealth)
            {
                _currentHealth.text = Mathf.RoundToInt(healthChangedEventArgs.NewHealth).ToString();
            }
        }
    }
}

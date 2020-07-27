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

        [SerializeField] AudioSource _damageTakenSound = null;
        [SerializeField] AudioSource _diedSound = null;

        public void OnHealthChanged(HealthChangedEventArgs healthChangedEventArgs)
        {
            if (_healthBar)
            {
                _healthBar.minValue = 0;
                _healthBar.maxValue = healthChangedEventArgs.MaxHealth;
                _healthBar.value = healthChangedEventArgs.CurrentHealth;
            }

            if (_currentHealth)
            {
                _currentHealth.text = Mathf.RoundToInt(healthChangedEventArgs.CurrentHealth).ToString();
            }

            if (_maxHealth)
            {
                _maxHealth.text = Mathf.RoundToInt(healthChangedEventArgs.MaxHealth).ToString();
            }
        }

        public void OnDamageTaken()
        {
            _damageTakenSound.Play();
        }

        public void OnDied()
        {
            _diedSound.Play();
        }
    }
}

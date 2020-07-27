/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

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

        public void OnHealthChanged(float health)
        {
            if (_healthBar)
            {
                _healthBar.value = health;
            }

            if (_currentHealth)
            {
                _currentHealth.text = Mathf.RoundToInt(health).ToString();
            }
        }

        public void OnMaxHealthChanged(float maxHealth)
        {
            if (_healthBar)
            {
                _healthBar.minValue = 0;
                _healthBar.maxValue = maxHealth;

            }

            if (_maxHealth)
            {
                _maxHealth.text = Mathf.RoundToInt(maxHealth).ToString();
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

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
    public class ShieldUI : MonoBehaviour
    {
        [SerializeField] Slider _shield = null;
        [SerializeField] Text _currentShieldCharge = null;
        [SerializeField] Text _maxShieldCharge = null;

        [SerializeField] AudioSource _shieldDamagedSound = null;
        [SerializeField] AudioSource _shieldDestroyedSound = null;
        [SerializeField] AudioSource _shieldPartlyRechargeSound = null;

        public void OnShieldChanged(ShieldChangedEventArgs shieldChangedEventArgs)
        {
            if (_shield)
            {
                _shield.minValue = 0;
                _shield.maxValue = shieldChangedEventArgs.MaxCharge;
                _shield.value = shieldChangedEventArgs.CurrentCharge;
            }

            if (_currentShieldCharge)
            {
                _currentShieldCharge.text = Mathf.RoundToInt(shieldChangedEventArgs.CurrentCharge).ToString();
            }

            if (_maxShieldCharge)
            {
                _maxShieldCharge.text = Mathf.RoundToInt(shieldChangedEventArgs.MaxCharge).ToString();
            }
        }

        public void OnDied()
        {
            _shieldDestroyedSound.Stop();
        }

        public void OnShieldDamaged()
        {
            _shieldDamagedSound.Play();
        }

        public void OnShieldDestroyed()
        {
            _shieldDestroyedSound.Play();
        }

        public void OnShieldPartlyRecharged()
        {
            _shieldDestroyedSound.Stop();
            _shieldPartlyRechargeSound.Play();
        }

        public void OnShieldFullyRecharged()
        {
            _shieldDestroyedSound.Stop();
        }
    }
}

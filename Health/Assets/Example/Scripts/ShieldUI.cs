/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

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

        public void OnShieldChargeChanged(float shieldCharge)
        {
            if (_shield)
            {
                _shield.value = shieldCharge;
            }

            if (_currentShieldCharge)
            {
                _currentShieldCharge.text = Mathf.RoundToInt(shieldCharge).ToString();
            }
        }

        public void OnMaxShieldChargeChanged(float maxShieldCharge)
        {
            if (_shield)
            {
                _shield.minValue = 0;
                _shield.maxValue = maxShieldCharge;
            }

            if (_maxShieldCharge)
            {
                _maxShieldCharge.text = Mathf.RoundToInt(maxShieldCharge).ToString();
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

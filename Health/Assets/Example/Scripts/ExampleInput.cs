/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

using Rakrae.Unity.Health.Behaviours;
using UnityEngine;

namespace Rakrae.Unity.Health.Example
{
    public class ExampleInput : MonoBehaviour
    {
        [SerializeField] private HealthSystem _healthSystem = null;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _healthSystem.ApplyDamage(10);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                _healthSystem.AddHealingEffect(new PeriodicHealingEffect(1, 5, true, 4));
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _healthSystem.AddDamageEffect(new PeriodicDamageEffect(1, 4, false, 10));
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                _healthSystem.ToggleSelfHealing();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                _healthSystem.ClearPeriodicEffects();
            }
        }
    }
}

/*
MIT License
Copyright (c) 2020 Raimund Krämer
For the full license text please refer to the LICENSE file.
 */

using System;

namespace Rakrae.Unity.Health
{
    public class Shield
    {
        public Shield(float maxCharge, float initialCharge, float blockPercentage)
        {
            CurrentCharge = maxCharge;
            MaxCharge = Math.Max(initialCharge, maxCharge);
            BlockPercentage = Math.Max(0.0f, Math.Min(blockPercentage, 100.0f));
        }

        public float CurrentCharge { get; private set; }
        public float MaxCharge { get; }
        public float BlockPercentage { get; }

        /// <summary>
        /// Try to block some damage. Blocking damage fails if the
        /// shield is already destroyed or if the shield is set up
        /// to block 0 %.
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="remainingDamage"></param>
        /// <returns>True if some amount of damage was blocked, false otherwise</returns>
        public bool TryBlockDamage(float damage, out float remainingDamage)
        {
            if (BlockPercentage == 0 || CurrentCharge == 0)
            {
                remainingDamage = damage;
                return false;
            }

            float blockedDamage = BlockPercentage / 100.0f * damage;
            blockedDamage = Math.Min(blockedDamage, CurrentCharge);

            CurrentCharge -= blockedDamage;
            CurrentCharge = Math.Max(CurrentCharge, 0);

            remainingDamage = damage - blockedDamage;
            return true;
        }

        public void Recharge(float amount)
        {
            CurrentCharge = Math.Min(CurrentCharge + amount, MaxCharge);
        }
    }
}

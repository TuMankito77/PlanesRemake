namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using System;

    using UnityEngine;

    [Serializable]
    public class SpeedBoosterAbilityData : AbilityData
    {
        [SerializeField, Min(1)]
        private int speedMultiplier = 2;

        public int SpeedMultiplier => speedMultiplier;
    }    
}

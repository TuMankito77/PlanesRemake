namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using System;

    using UnityEngine;

    [Serializable]
    public class CoinMultiplierAbilityData : AbilityData
    {
        [SerializeField]
        private int coinMultiplier = 2;

        [SerializeField]
        private Material[] materialsToSwap = new Material[0];

        public int CoinMultiplier => coinMultiplier;
        public Material[] MaterialsToSwap => materialsToSwap;
    }
}

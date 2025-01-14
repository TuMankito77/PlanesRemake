namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using System;
    
    using UnityEngine;
    
    [Serializable]
    public class MagnetAbilityData : AbilityData
    {
        [SerializeField]
        private float attractionSpeed = 10;

        [SerializeField]
        private string pickUpItemTag = string.Empty;

        public float AttractionSpeed => attractionSpeed;
        public string PickUpItemTag => pickUpItemTag;
    }
}
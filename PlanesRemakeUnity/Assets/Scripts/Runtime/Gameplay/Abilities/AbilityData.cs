namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using System;
    
    using UnityEngine;
    
    [Serializable]
    public class AbilityData
    {
        [SerializeField]
        private GameObject abilityVisualPrefabInstance = null;

        [SerializeField]
        private AnimationCurve transparencyOverTime = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField]
        private float duration = 5;

        public GameObject AbilityVisualPrefab => abilityVisualPrefabInstance;
        public AnimationCurve TransparencyOverTime => transparencyOverTime;
        public float Duration => duration;
    }
}

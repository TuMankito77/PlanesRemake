namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using UnityEngine;

    public class VisualAbility : BaseAbility
    {
        protected GameObject AbilityVisualPrefabInstance = null;
        private AnimationCurve transparencyOverTime = AnimationCurve.EaseInOut(0, 0, 1, 1);
        private Material abilityMaterial = null;

        protected override bool IsAbilityTimerTickEnabled => true;

        public VisualAbility(GameObject sourceOwner, AbilityData sourceAbilityData) : 
            base(sourceOwner, sourceAbilityData)
        {
            AbilityVisualPrefabInstance = GameObject.Instantiate(sourceAbilityData.AbilityVisualPrefab, owner.transform);
            MeshRenderer meshRenderer = AbilityVisualPrefabInstance.GetComponent<MeshRenderer>();
            abilityMaterial = meshRenderer.material;
            abilityMaterial.SetFloat("_Transparency", 0);
            transparencyOverTime = sourceAbilityData.TransparencyOverTime;
        }

        protected override void OnAbilityTimerTick(float deltaTime, float timeTranscurred)
        {
            base.OnAbilityTimerTick(deltaTime, timeTranscurred);
            abilityMaterial.SetFloat("_Transparency", transparencyOverTime.Evaluate(timeTranscurred / activeTimer.Duration));
        }

        public override void Deactivate()
        {
            base.Deactivate();
            GameObject.Destroy(AbilityVisualPrefabInstance);
        }
    }
}


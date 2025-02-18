namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using System.Collections.Generic;

    using UnityEngine;

    public abstract class VisualAbility : BaseAbility
    {
        protected GameObject abilityVisualPrefabInstance = null;
        private AnimationCurve transparencyOverTime = AnimationCurve.EaseInOut(0, 0, 1, 1);
        private List<Material> abilityMaterials = null;

        protected override bool IsAbilityTimerTickEnabled => true;

        public VisualAbility(GameObject sourceOwner, AbilityData sourceAbilityData) : 
            base(sourceOwner, sourceAbilityData)
        {
            abilityVisualPrefabInstance = GameObject.Instantiate(sourceAbilityData.AbilityVisualPrefab, owner.transform);
            abilityMaterials = new List<Material>();
            MeshRenderer meshRenderer = abilityVisualPrefabInstance.GetComponent<MeshRenderer>();

            if(meshRenderer != null)
            {
                abilityMaterials.Add(meshRenderer.material);
            }

            UpdateMaterialsTransparency(0);
            transparencyOverTime = sourceAbilityData.TransparencyOverTime;
        }

        protected override void OnAbilityTimerTick(float deltaTime, float timeTranscurred)
        {
            base.OnAbilityTimerTick(deltaTime, timeTranscurred);
            UpdateMaterialsTransparency(transparencyOverTime.Evaluate(timeTranscurred / activeTimer.Duration));
        }

        public override void Deactivate()
        {
            base.Deactivate();
            abilityMaterials.Clear();
            abilityMaterials = null;
            GameObject.Destroy(abilityVisualPrefabInstance);
        }

        private void UpdateMaterialsTransparency(float transparency)
        {
            foreach(Material material in abilityMaterials)
            {
                material.SetFloat("_Transparency", transparency);
            }
        }
    }
}


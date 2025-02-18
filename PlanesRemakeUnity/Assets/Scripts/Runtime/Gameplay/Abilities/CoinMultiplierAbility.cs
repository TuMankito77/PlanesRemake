namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using System.Collections.Generic;

    using UnityEngine;
    
    using PlanesRemake.Runtime.Events;

    public class CoinMultiplierAbility : VisualAbility
    {
        private  struct MeshRendererMaterialsLink
        {
            public MeshRenderer meshRenderer;
            public Material[] materials;

            public MeshRendererMaterialsLink(MeshRenderer sourceMeshRenderer, Material[] sourceMaterials)
            {
                meshRenderer = sourceMeshRenderer;
                materials = sourceMaterials;
            }
        }

        private ParticleSystem particleSystem = null;
        private List<MeshRendererMaterialsLink> meshRendererMaterialsLinks = null;
        private Material[] materialsToSwap = new Material[0];

        public int CoinMultiplier { get; private set; } 

        public CoinMultiplierAbility(GameObject sourceOwner, CoinMultiplierAbilityData sourceAbilityData, MeshRenderer[] sourceMeshRenderers) : base(sourceOwner, sourceAbilityData)
        {
            particleSystem = abilityVisualPrefabInstance.GetComponent<ParticleSystem>();
            particleSystem.Play();
            CoinMultiplier = sourceAbilityData.CoinMultiplier;

            meshRendererMaterialsLinks = new List<MeshRendererMaterialsLink>();

            foreach(MeshRenderer meshRenderer in sourceMeshRenderers)
            {
                MeshRendererMaterialsLink meshRendererMaterialsLink = new MeshRendererMaterialsLink(meshRenderer, meshRenderer.materials);
                meshRendererMaterialsLinks.Add(meshRendererMaterialsLink);
            }

            materialsToSwap = sourceAbilityData.MaterialsToSwap;
        }

        public override void Activate()
        {
            base.Activate();

            foreach (MeshRendererMaterialsLink meshRendererMaterialsLink in meshRendererMaterialsLinks)
            {
                meshRendererMaterialsLink.meshRenderer.materials = materialsToSwap;
            }

            EventDispatcher.Instance.Dispatch(AbilityEvents.OnCoinMultiplierActivated, this);
        }

        public override void Deactivate()
        {
            //We call this before since the prefab instance is destroyed on the parent's function implementation.
            particleSystem.Stop();
            base.Deactivate();

            foreach(MeshRendererMaterialsLink meshRendererMaterialsLink in meshRendererMaterialsLinks)
            {
                meshRendererMaterialsLink.meshRenderer.materials = meshRendererMaterialsLink.materials;
            }

            EventDispatcher.Instance.Dispatch(AbilityEvents.OnCoinMultiplierDeactivated, this);
        }
    }
}

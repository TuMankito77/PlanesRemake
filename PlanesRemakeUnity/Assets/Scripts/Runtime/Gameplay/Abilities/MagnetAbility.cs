namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using System;
    using System.Collections.Generic;
    
    using UnityEngine;
    
    using PlanesRemake.Runtime.Gameplay.CommonBehaviors;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Utils;

    public class MagnetAbility : BaseAbility, IListener
    {
        private float attractionSpeed = 10;
        private GameObject magnetAbilityPrefabInstance = null;
        private AnimationCurve transparencyOverTime = AnimationCurve.EaseInOut(0, 0, 1, 1);
        private CollisionEventNotifier coinCollisionDetection = null;
        private List<BasePickUpItem> pickUpItemsAttracted = null;
        private Material abilityMaterial = null;
        private string pickUpItemTag = string.Empty;

        protected override bool IsAbilityTimerTickEnabled => true;

        public MagnetAbility(GameObject sourceOwner, float sourceDurationInSeconds, MagnetAbilityData sourceMagnetAbilityData)
            :base(sourceOwner, sourceMagnetAbilityData)
        {
            pickUpItemsAttracted = new List<BasePickUpItem>();
            magnetAbilityPrefabInstance = GameObject.Instantiate(sourceMagnetAbilityData.AbilityVisualPrefab, owner.transform);
            coinCollisionDetection = magnetAbilityPrefabInstance.GetComponent<CollisionEventNotifier>();
            MeshRenderer meshRenderer = magnetAbilityPrefabInstance.GetComponent<MeshRenderer>();
            abilityMaterial = meshRenderer.material;
            abilityMaterial.SetFloat("_Transparency", 0);
            transparencyOverTime = sourceMagnetAbilityData.TransparencyOverTime;
            attractionSpeed = sourceMagnetAbilityData.AttractionSpeed;
            pickUpItemTag = sourceMagnetAbilityData.PickUpItemTag;
        }

        public override void Activate()
        {
            base.Activate();
            EventDispatcher.Instance.AddListener(this, typeof(GameplayEvents));
            coinCollisionDetection.OnTiggerEnterDetected += OnCoinEnteredAttractingField;
        }

        public override void Deactivate()
        {
            coinCollisionDetection.OnTiggerEnterDetected -= OnCoinEnteredAttractingField;
            
            foreach(BasePickUpItem pickUpItem in pickUpItemsAttracted)
            {
                pickUpItem.DirectionalMovementComponent.enabled = true;
            }

            pickUpItemsAttracted.Clear();
            pickUpItemsAttracted = null;
            GameObject.Destroy(magnetAbilityPrefabInstance);
            EventDispatcher.Instance.RemoveListener(this, typeof(GameplayEvents));
            base.Deactivate();
        }

        protected override void OnAbilityTimerTick(float deltaTime, float timeTranscurred)
        {
            base.OnAbilityTimerTick(deltaTime, timeTranscurred);

            abilityMaterial.SetFloat("_Transparency", transparencyOverTime.Evaluate(timeTranscurred / activeTimer.Duration));

            for (int i = 0; i < pickUpItemsAttracted.Count; i++)
            {
                BasePickUpItem pickUpItem = pickUpItemsAttracted[i];
                Vector3 moveDirection = owner.transform.position - pickUpItem.transform.position;
                moveDirection.Normalize();
                Vector3 velocity = moveDirection * deltaTime * attractionSpeed;
                pickUpItem.transform.position += velocity; 
            }
        }

        private void OnCoinEnteredAttractingField(Collider collider)
        {
            if(collider.gameObject.tag != pickUpItemTag)
            {
                return;
            }

            BasePickUpItem pickUpItem = collider.gameObject.GetComponent<BasePickUpItem>();
            pickUpItem.DirectionalMovementComponent.enabled = false;
            pickUpItemsAttracted.Add(pickUpItem);
        }

        #region IListener

        public void HandleEvent(IComparable eventName, object data)
        {
            switch(eventName)
            {
                case GameplayEvents gameplayEvent:
                    {
                        HandleGameplayEvents(gameplayEvent, data);
                        break;
                    }

                default:
                    {
                        LoggerUtil.LogError($"{GetType()} - The event {eventName} is not handled by this class. You may need to unsubscribe.");
                        break;
                    }
            }
        }

        #endregion

        private void HandleGameplayEvents(GameplayEvents gameplayEvent, object data)
        {
            switch(gameplayEvent)
            {
                case GameplayEvents.OnCoinCollected:
                    {
                        BasePickUpItem basePickUpItem = data as BasePickUpItem;
                        
                        if(pickUpItemsAttracted.Contains(basePickUpItem))
                        {
                            pickUpItemsAttracted.Remove(basePickUpItem);
                        }

                        break;
                    }
            }
        }
    }
}

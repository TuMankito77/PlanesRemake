namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using System;
    using System.Collections.Generic;
    
    using UnityEngine;
    
    using PlanesRemake.Runtime.Gameplay.CommonBehaviors;
    using PlanesRemake.Runtime.Events;

    public class MagnetAbility : VisualAbility, IListener
    {
        private float attractionSpeed = 10;
        private CollisionEventNotifier coinCollisionDetection = null;
        private List<BasePickUpItem> pickUpItemsAttracted = null;
        private string pickUpItemTag = string.Empty;

        protected override bool IsAbilityTimerTickEnabled => true;

        public MagnetAbility(GameObject sourceOwner, MagnetAbilityData sourceMagnetAbilityData)
            :base(sourceOwner, sourceMagnetAbilityData)
        {
            pickUpItemsAttracted = new List<BasePickUpItem>();
            coinCollisionDetection = AbilityVisualPrefabInstance.GetComponent<CollisionEventNotifier>();
            attractionSpeed = sourceMagnetAbilityData.AttractionSpeed;
            pickUpItemTag = sourceMagnetAbilityData.PickUpItemTag;
            eventsToListenFor.Add(typeof(GameplayEvents));
        }

        public override void Activate()
        {
            base.Activate();
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
            base.Deactivate();
        }

        protected override void OnAbilityTimerTick(float deltaTime, float timeTranscurred)
        {
            base.OnAbilityTimerTick(deltaTime, timeTranscurred);

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

        public override void HandleEvent(IComparable eventName, object data)
        {
            base.HandleEvent(eventName, data);

            switch(eventName)
            {
                case GameplayEvents gameplayEvent:
                    {
                        HandleGameplayEvents(gameplayEvent, data);
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

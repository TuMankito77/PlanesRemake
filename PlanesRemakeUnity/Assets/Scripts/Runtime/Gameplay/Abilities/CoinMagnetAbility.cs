namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using System;
    using System.Collections.Generic;
    
    using UnityEngine;
    
    using PlanesRemake.Runtime.Gameplay.CommonBehaviors;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Utils;
    using PlanesRemake.Runtime.Core;

    public class CoinMagnetAbility : BaseAbility, IListener
    {
        private string MAGNET_ABILITY_PREFAP_PATH = "MainLevel/Abilities/CoinMagnet";

        private CollisionEventNotifier coinCollisionDetection = null;
        private List<BasePickUpItem> pickUpItemsAttracted = null;
        private float attractionSpeed = 10;
        private GameObject magnetAbilityPrefabInstance = null;
        private ContentLoader contentLoader = null;

        protected override bool IsAbilityTimerTickEnabled => true;

        public CoinMagnetAbility(GameObject sourceOwner, float sourceDurationInSeconds, ContentLoader sourceContentLoader)
            :base(sourceOwner, sourceDurationInSeconds)
        {
            contentLoader = sourceContentLoader;
            pickUpItemsAttracted = new List<BasePickUpItem>();
            GameObject magnetAbilityPrefab = contentLoader.LoadAssetSynchronously<GameObject>(MAGNET_ABILITY_PREFAP_PATH);
            magnetAbilityPrefabInstance = GameObject.Instantiate(magnetAbilityPrefab, owner.transform);
            coinCollisionDetection = magnetAbilityPrefabInstance.GetComponent<CollisionEventNotifier>();
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

            for(int i = 0; i < pickUpItemsAttracted.Count; i++)
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
            if(collider.gameObject.tag != "Coin")
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

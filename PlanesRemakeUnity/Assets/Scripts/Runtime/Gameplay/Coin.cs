namespace PlanesRemake.Runtime.Gameplay
{
    using System;
    
    using UnityEngine;
    
    using PlanesRemake.Runtime.Gameplay.CommonBehaviors;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Utils;
    using UnityEngine.Pool;

    [RequireComponent(typeof(DirectionalMovement), typeof(CollisionEventNotifier), typeof(DirectionalSpinning))]
    public class Coin : MonoBehaviour, IListener
    {
        private CollisionEventNotifier collisionEventNotifier = null;
        private DirectionalMovement directionalMovement = null;
        private DirectionalSpinning directionalSpinning = null;
        private string triggerDetectionTag = string.Empty;

        #region Unity Methods

        private void Awake()
        {
            collisionEventNotifier = GetComponent<CollisionEventNotifier>();
            directionalMovement = GetComponent<DirectionalMovement>();
            directionalSpinning = GetComponent<DirectionalSpinning>();
        }

        private void Start()
        {
            collisionEventNotifier.OnTiggerEnterDetected += OnCoinTriggerEntered;
        }

        private void OnDestroy()
        {
            collisionEventNotifier.OnTiggerEnterDetected -= OnCoinTriggerEntered;
            EventDispatcher.Instance.RemoveListener(this, typeof(UiEvents), typeof(GameplayEvents));
        }

        #endregion

        #region IListener

        public void HandleEvent(IComparable eventName, object data)
        {
            switch(eventName)
            {
                case UiEvents uiEvent:
                    {
                        HandleUiEvents(uiEvent, data);
                        break;
                    }

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

        #region IPoolableObjec

        #endregion

        public void Initialize(string sourceTriggerDetectionTag)
        {
            triggerDetectionTag = sourceTriggerDetectionTag;
            EventDispatcher.Instance.AddListener(this, typeof(UiEvents), typeof(GameplayEvents));
        }

        private void OnCoinTriggerEntered(Collider other)
        {
            if(other.tag == triggerDetectionTag)
            {
                EventDispatcher.Instance.Dispatch(GameplayEvents.OnCoinCollected, other);
                StartDestroySequence();
            }
        }

        private void StartDestroySequence()
        {
            SetMovementEnabled(false);
            //Play destroy animation or spawn VFX.
            //NOTE: Replace this by the pooling system.
            Destroy(gameObject);
        }

        private void SetMovementEnabled(bool isEnabled)
        {
            collisionEventNotifier.enabled = isEnabled;
            directionalMovement.enabled = isEnabled;
            directionalSpinning.enabled = isEnabled;
        }

        private void HandleUiEvents(UiEvents uiEvent, object data)
        {
            switch(uiEvent)
            {
                case UiEvents.OnPauseButtonPressed:
                    {
                        collisionEventNotifier.enabled = false;
                        directionalMovement.enabled = false;
                        break;
                    }

                case UiEvents.OnUnpauseButtonPressed:
                    {
                        collisionEventNotifier.enabled = true;
                        directionalMovement.enabled = true;
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        private void HandleGameplayEvents(GameplayEvents gameplayEvent, object data)
        {
            switch(gameplayEvent)
            {
                case GameplayEvents.OnWallcollision:
                    {
                        SetMovementEnabled(false);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
    }
}

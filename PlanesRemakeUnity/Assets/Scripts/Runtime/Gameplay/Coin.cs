namespace PlanesRemake.Runtime.Gameplay
{
    using System;
    
    using UnityEngine;
    
    using PlanesRemake.Runtime.Gameplay.CommonBehaviors;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Utils;
    using UnityEngine.Pool;
    using PlanesRemake.Runtime.Sound;

    public class Coin : BasePoolableObject, IListener
    {
        [SerializeField]
        private CollisionEventNotifier collisionEventNotifier = null;

        [SerializeField]
        private DirectionalMovement directionalMovement = null;

        [SerializeField]
        private DirectionalSpinning directionalSpinning = null;

        [SerializeField]
        private ObjectPoolReleaser objectPoolReleaser = null;
        
        private IObjectPool<BasePoolableObject> coinsPool = null;
        private string triggerDetectionTag = string.Empty;
        private AudioManager audioManager = null;

        protected override IObjectPool<BasePoolableObject> ObjectPool => coinsPool;


        #region Uinty Methods

        private void OnEnable()
        {
            collisionEventNotifier.OnTiggerEnterDetected += OnCoinTriggerEntered;
            EventDispatcher.Instance.AddListener(this, typeof(UiEvents), typeof(GameplayEvents));
        }

        private void OnDisable()
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

        public void Initialize(string sourceTriggerDetectionTag, 
            IObjectPool<BasePoolableObject> sourceCoinPool, 
            CameraExtensions.Boundaries cameraBoundaries,
            AudioManager sourceAudioManager)
        {
            triggerDetectionTag = sourceTriggerDetectionTag;
            coinsPool = sourceCoinPool;
            audioManager = sourceAudioManager;
            objectPoolReleaser.SetCameraBoundaries(cameraBoundaries);
            SetMovementEnabled(true);
        }

        private void OnCoinTriggerEntered(Collider other)
        {
            if(other.tag == triggerDetectionTag)
            {
                EventDispatcher.Instance.Dispatch(GameplayEvents.OnCoinCollected, this);
                StartDestroySequence();
            }
        }

        private void StartDestroySequence()
        {
            SetMovementEnabled(false);
            //Play destroy animation or spawn VFX.
            audioManager.PlayGameplayClip(ClipIds.COIN_CLIP);
            ReleaseObject();
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

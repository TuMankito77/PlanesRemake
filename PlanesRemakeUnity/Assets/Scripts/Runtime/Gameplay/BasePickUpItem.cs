namespace PlanesRemake.Runtime.Gameplay
{
    using System;

    using UnityEngine;

    using PlanesRemake.Runtime.Gameplay.CommonBehaviors;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Utils;
    using UnityEngine.Pool;
    using PlanesRemake.Runtime.Sound;

    public abstract class BasePickUpItem : BasePoolableObject, IListener
    {
        [SerializeField]
        private CollisionEventNotifier collisionEventNotifier = null;

        [SerializeField]
        private DirectionalMovement directionalMovement = null;

        [SerializeField]
        private DirectionalSpinning directionalSpinning = null;

        [SerializeField]
        private ObjectPoolReleaser objectPoolReleaser = null;

        private IObjectPool<BasePoolableObject> poolContainer = null;
        private string triggerDetectionTag = string.Empty;
        private AudioManager audioManager = null;

        protected override IObjectPool<BasePoolableObject> ObjectPool => poolContainer;
        protected abstract GameplayEvents GameplayEventToDispatch { get; }
        protected abstract string PickUpClipId { get; }

        #region Uinty Methods

        private void OnEnable()
        {
            collisionEventNotifier.OnTiggerEnterDetected += OnTriggerEntered;
            EventDispatcher.Instance.AddListener(this, typeof(UiEvents), typeof(GameplayEvents));
        }

        private void OnDisable()
        {
            collisionEventNotifier.OnTiggerEnterDetected -= OnTriggerEntered;
            EventDispatcher.Instance.RemoveListener(this, typeof(UiEvents), typeof(GameplayEvents));
        }

        #endregion

        #region IListener

        public void HandleEvent(IComparable eventName, object data)
        {
            switch (eventName)
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

        public void Initialize(string sourceTriggerDetectionTag,
            IObjectPool<BasePoolableObject> sourcePoolContainer,
            CameraBoundaries cameraBoundaries,
            AudioManager sourceAudioManager)
        {
            triggerDetectionTag = sourceTriggerDetectionTag;
            poolContainer = sourcePoolContainer;
            audioManager = sourceAudioManager;
            objectPoolReleaser.SetCameraBoundaries(cameraBoundaries);
            SetMovementEnabled(true);
        }

        private void OnTriggerEntered(Collider other)
        {
            if (other.tag == triggerDetectionTag)
            {
                EventDispatcher.Instance.Dispatch(GameplayEventToDispatch, this);
                StartDestroySequence();
            }
        }

        private void StartDestroySequence()
        {
            SetMovementEnabled(false);
            //Play destroy animation or spawn VFX.
            audioManager.PlayGameplayClip(PickUpClipId);
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
            switch (uiEvent)
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
            switch (gameplayEvent)
            {
                case GameplayEvents.OnWallcollision:
                    {
                        SetMovementEnabled(false);
                        collisionEventNotifier.OnTiggerEnterDetected -= OnTriggerEntered;
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

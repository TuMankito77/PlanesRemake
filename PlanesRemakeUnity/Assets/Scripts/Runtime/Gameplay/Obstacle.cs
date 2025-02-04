namespace PlanesRemake.Runtime.Gameplay
{
    using System;
    
    using UnityEngine;
    using UnityEngine.Pool;
    
    using PlanesRemake.Runtime.Gameplay.CommonBehaviors;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Utils;

    [RequireComponent(typeof(DirectionalMovement), typeof(OsilateMovement))]
    public class Obstacle : BasePoolableObject, IListener
    {
        [SerializeField]
        private CollisionEventNotifier gapCollider = null;

        [SerializeField]
        private CollisionEventNotifier[] wallColliders = null;

        [SerializeField]
        private DirectionalMovement directionalMovement = null;

        [SerializeField]
        private OsilateMovement osilateMovement = null;

        [SerializeField]
        private ObjectPoolReleaser objectPoolReleaser = null;
        
        private bool wasCrossed = false;
        private string crashTriggerDetectionTag = string.Empty;
        private string evadedTriggerDetectionTag = string.Empty;
        private IObjectPool<BasePoolableObject> obstaclesPool = null;

        protected override IObjectPool<BasePoolableObject> ObjectPool => obstaclesPool;

        #region Unity Methods

        private void OnEnable()
        {
            foreach (CollisionEventNotifier wallCollider in wallColliders)
            {
                wallCollider.OnTiggerEnterDetected += OnWallTriggerEnter;
                wallCollider.OnTriggerExitDetected += OnWallTriggerExit;
            }

            gapCollider.OnTriggerExitDetected += OnGapTriggerExited;
            EventDispatcher.Instance.AddListener(this, typeof(UiEvents), typeof(GameplayEvents));
        }

        private void OnDisable()
        {
            foreach (CollisionEventNotifier wallCollider in wallColliders)
            {
                wallCollider.OnTiggerEnterDetected -= OnWallTriggerEnter;
                wallCollider.OnTriggerExitDetected -= OnWallTriggerExit;
            }

            gapCollider.OnTriggerExitDetected -= OnGapTriggerExited;
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

        //NOTE: We can create a container that has all of this information so that it gets passed in
        //and we do not have this long list of parameters 
        public void Initialize(string sourceCrashTriggerDetectionTag, string sourceEvadedTriggerDetectionTag, IObjectPool<BasePoolableObject> sourceObstaclesPool, CameraBoundaries cameraBoundaries, float movementSpeed, float osilationSpeed, float osilationDistance)
        {
            crashTriggerDetectionTag = sourceCrashTriggerDetectionTag;
            evadedTriggerDetectionTag = sourceEvadedTriggerDetectionTag;
            obstaclesPool = sourceObstaclesPool;
            objectPoolReleaser.SetCameraBoundaries(cameraBoundaries);
            Vector3 velocityVector = new Vector3(-movementSpeed, 0, 0);
            directionalMovement.ChangeVelocityVector(velocityVector);
            osilateMovement.ChangeOsilationDistance(osilationDistance);
            osilateMovement.ChangeSpeed(osilationSpeed);
            SetMovementEnabled(true);
            wasCrossed = false;
        }

        private void OnWallTriggerEnter(Collider other)
        {
            if(other.tag == crashTriggerDetectionTag)
            {
                EventDispatcher.Instance.Dispatch(GameplayEvents.OnWallcollision, other);
            }
        }

        private void OnWallTriggerExit(Collider other)
        {
            MarkWallAsEvaded(other);
        }

        private void OnGapTriggerExited(Collider other)
        {
            MarkWallAsEvaded(other);
        }

        private void MarkWallAsEvaded(Collider other)
        {
            if (other.tag == evadedTriggerDetectionTag && !wasCrossed &&
                other.transform.position.x > transform.position.x)
            {
                wasCrossed = true;
                EventDispatcher.Instance.Dispatch(GameplayEvents.OnWallEvaded, this);
            }
        }

        private void SetMovementEnabled(bool isEnabled)
        {
            directionalMovement.enabled = isEnabled;
            osilateMovement.enabled = isEnabled;
        }

        private void HandleUiEvents(UiEvents uiEvent, object data)
        {
            switch(uiEvent)
            {
                case UiEvents.OnPauseButtonPressed:
                    {
                        SetMovementEnabled(false);
                        break;
                    }

                case UiEvents.OnUnpauseButtonPressed:
                    {
                        SetMovementEnabled(true);
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

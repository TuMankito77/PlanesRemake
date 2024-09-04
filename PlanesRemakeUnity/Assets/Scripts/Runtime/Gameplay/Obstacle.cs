namespace PlanesRemake.Runtime.Gameplay
{
    using System;
    
    using UnityEngine;
    
    using PlanesRemake.Runtime.Gameplay.CommonBehaviors;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Utils;
    using UnityEngine.Pool;

    [RequireComponent(typeof(DirectionalMovement), typeof(OsilateMovement))]
    public class Obstacle : BasePoolableObject, IListener
    {
        [SerializeField]
        private CollisionEventNotifier gapCollider = null;

        [SerializeField]
        private CollisionEventNotifier[] wallColliders = null;
        
        private DirectionalMovement directionalMovement = null;
        private OsilateMovement osilateMovement = null;
        private ObjectPoolReleaser objectPoolReleaser = null;
        private bool wasCrossed = false;
        private string triggerDetectionTag = string.Empty;
        private IObjectPool<GameObject> obstaclesPool = null;

        protected override IObjectPool<GameObject> ObjectPool => obstaclesPool;

        #region Unity Methods

        private void Awake()
        {
            directionalMovement = GetComponent<DirectionalMovement>();
            osilateMovement = GetComponent<OsilateMovement>();
            objectPoolReleaser = GetComponent<ObjectPoolReleaser>();
        }

        private void Start()
        {
            foreach(CollisionEventNotifier wallCollider in wallColliders)
            {
                wallCollider.OnTiggerEnterDetected += OnWallTriggerEntered;
            }

            gapCollider.OnTriggerExitDetected += OnGapTriggerExited;
        }

        private void OnDestroy()
        {
            foreach(CollisionEventNotifier wallCollider in wallColliders)
            {
                wallCollider.OnTiggerEnterDetected -= OnWallTriggerEntered;
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
        public void Initialize(string sourceTriggerDetectionTag, IObjectPool<GameObject> sourceObstaclesPool, CameraExtensions.Boundaries cameraBoundaries, float movementSpeed, float osilationSpeed, float osilationDistance, Vector3 startingPosition, Quaternion startingRotation)
        {
            triggerDetectionTag = sourceTriggerDetectionTag;
            obstaclesPool = sourceObstaclesPool;
            objectPoolReleaser.SetCameraBoundaries(cameraBoundaries);
            transform.position = startingPosition;
            transform.rotation = startingRotation;
            Vector3 velocityVector = new Vector3(-movementSpeed, 0, 0);
            directionalMovement.ChangeVelocityVector(velocityVector);
            osilateMovement.ChangeOsilationDistance(osilationDistance);
            osilateMovement.ChangeSpeed(osilationSpeed);
            SetMovementEnabled(true);
            EventDispatcher.Instance.AddListener(this, typeof(UiEvents), typeof(GameplayEvents));
        }

        public override void ReleaseObject()
        {
            EventDispatcher.Instance.RemoveListener(this, typeof(UiEvents), typeof(GameplayEvents));
            base.ReleaseObject();
        }

        private void OnWallTriggerEntered(Collider other)
        {
            if(other.tag == triggerDetectionTag)
            {
                EventDispatcher.Instance.Dispatch(GameplayEvents.OnWallcollision, other);
            }
        }

        private void OnGapTriggerExited(Collider other)
        {
            if(other.tag == triggerDetectionTag && !wasCrossed &&
                other.transform.position.x > transform.position.x)
            {
                wasCrossed = true;
                EventDispatcher.Instance.Dispatch(GameplayEvents.OnWallEvaded, other);
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

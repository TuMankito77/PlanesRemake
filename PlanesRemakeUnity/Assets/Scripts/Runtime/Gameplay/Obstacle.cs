namespace PlanesRemake.Runtime.Gameplay
{
    using System;
    
    using UnityEngine;
    
    using PlanesRemake.Runtime.Gameplay.CommonBehaviors;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Utils;

    [RequireComponent(typeof(DirectionalMovement), typeof(OsilateMovement))]
    public class Obstacle : MonoBehaviour, IListener
    {
        [SerializeField]
        private CollisionEventNotifier gapCollider = null;

        [SerializeField]
        private CollisionEventNotifier[] wallColliders = null;
        
        private DirectionalMovement directionalMovement = null;
        private OsilateMovement osilateMovement = null;
        private bool wasCrossed = false;
        private string triggerDetectionTag = string.Empty;

        #region Unity Methods

        private void Awake()
        {
            directionalMovement = GetComponent<DirectionalMovement>();
            osilateMovement = GetComponent<OsilateMovement>();
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
            EventDispatcher.Instance.RemoveListener(this, typeof(UiEvents));
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
        public void Initialize(string sourceTriggerDetectionTag,float movementSpeed, float osilationSpeed, float osilationDistance, Vector3 startingPosition, Quaternion startingRotation)
        {
            triggerDetectionTag = sourceTriggerDetectionTag;
            transform.position = startingPosition;
            transform.rotation = startingRotation;
            Vector3 velocityVector = new Vector3(-movementSpeed, 0, 0);
            directionalMovement.ChangeVelocityVector(velocityVector);
            osilateMovement.ChangeOsilationDistance(osilationDistance);
            osilateMovement.ChangeSpeed(osilationSpeed);
            EventDispatcher.Instance.AddListener(this, typeof(UiEvents));
        }

        private void OnWallTriggerEntered(Collider other)
        {
            if(other.tag == triggerDetectionTag)
            {
                directionalMovement.enabled = false;
                osilateMovement.enabled = false;
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

        private void HandleUiEvents(UiEvents uiEvent, object data)
        {
            switch(uiEvent)
            {
                case UiEvents.OnPauseButtonPressed:
                    {
                        directionalMovement.enabled = false;
                        osilateMovement.enabled = false;
                        break;
                    }

                case UiEvents.OnUnpauseButtonPressed:
                    {
                        directionalMovement.enabled = true;
                        osilateMovement.enabled = true;
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

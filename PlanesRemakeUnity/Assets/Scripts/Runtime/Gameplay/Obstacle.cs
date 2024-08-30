namespace PlanesRemake.Runtime.Gameplay
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Gameplay.CommonBehaviors;
    using PlanesRemake.Runtime.Events;

    [RequireComponent(typeof(DirectionalMovement), typeof(OsilateMovement))]
    public class Obstacle : MonoBehaviour
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
        }

        #endregion

        //NOTE: We can create a container that has all of this information so that it gets passed in
        //and we do not have this long list of parameters 
        public void Initialize(string triggerCollisionDetectionTag,float movementSpeed, float osilationSpeed, float osilationDistance, Vector3 startingPosition, Quaternion startingRotation)
        {
            triggerDetectionTag = triggerCollisionDetectionTag;
            transform.position = startingPosition;
            transform.rotation = startingRotation;
            Vector3 velocityVector = new Vector3(-movementSpeed, 0, 0);
            directionalMovement.ChangeVelocityVector(velocityVector);
            osilateMovement.ChangeOsilationDistance(osilationDistance);
            osilateMovement.ChangeSpeed(osilationSpeed);
        }

        private void OnWallTriggerEntered(Collider other)
        {
            if(other.tag == triggerDetectionTag)
            {
                directionalMovement.enabled = false;
                osilateMovement.enabled = false;
                EventDispatcher.Instance.Dispatch(GameplayEvents.OnWallcollision, other);
                Debug.LogWarning("Aircraft crashed");
            }
        }

        private void OnGapTriggerExited(Collider other)
        {
            if(other.tag == triggerDetectionTag && !wasCrossed)
            {
                wasCrossed = true;
                EventDispatcher.Instance.Dispatch(GameplayEvents.OnGapCrossed, other);
                Debug.LogWarning("Aircraft crossed wall");
            }
        }
    }
}

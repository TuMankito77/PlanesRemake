namespace PlanesRemake.Runtime.Gameplay
{
    using System;
    
    using UnityEngine;
    
    using PlanesRemake.Runtime.Input;
    using PlanesRemake.Runtime.Utils;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Sound;

    public class Aircraft : MonoBehaviour, IInputControlableEntity, IListener
    {
        //NOTE: This is momentaneous, we have to make this tag
        //changeble from a drop-down menu on each object that uses it.
        public static string AIRCRAFT_TAG = "Aircraft";

        [SerializeField, Min(1)]
        private float movementSpeed = 30;

        [SerializeField, Min(1)]
        private float acceleration = 10;

        [SerializeField]
        private ParticleSystem vfxAircraftCrashed = null;

        [SerializeField]
        private MeshRenderer[] meshRenderersToHideWhenCrashing = null;

        private Vector2 direction = Vector2.zero;
        private CameraExtensions.Boundaries boundaries = default(CameraExtensions.Boundaries);
        private AudioManager audioManager = null;
        private float horizontalSpeed = 0;
        private float verticalSpeed = 0;
        //NOTE: Remove this timer once we have an animation an we know when the destroy animation finishes.
        private Timer timer = null;

        #region Unity Methods

        private void Update()
        {
            Vector3 currentVelocity = CalculateVelocity();
            Debug.LogWarning(currentVelocity.magnitude);
            Vector3 velocityOverTime = currentVelocity * Time.deltaTime;
            Vector3 newPosition = transform.position + velocityOverTime;

            transform.position = new Vector3(
                Mathf.Clamp(newPosition.x, boundaries.left, boundaries.right),
                Mathf.Clamp(newPosition.y, boundaries.bottom, boundaries.top),
                transform.position.z);
        }

        private void OnEnable()
        {
            EventDispatcher.Instance.AddListener(this, typeof(GameplayEvents));
        }

        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener(this, typeof(GameplayEvents));
        }

        #endregion

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
            }
        }

        #endregion

        public void Initialize(Camera sourceIsometricCamera, AudioManager sourceAudioManager)
        {
            boundaries = sourceIsometricCamera.GetScreenBoundariesInWorld(transform.position);
            audioManager = sourceAudioManager;
            audioManager.PlayLoopingClip(GetInstanceID(), ClipIds.AIRCRAFT_ENGINE_CLIP, transform, true);
            transform.position = boundaries.center;
        }

        public void UpdateDirection(Vector2 sourceDirection)
        {
           direction = sourceDirection.normalized;
        }

        public void Dispose()
        {
            audioManager.StopLoopingClip(GetInstanceID());
        }

        private void DestroyAircraft()
        {
            direction = Vector2.zero;
            audioManager.PauseLoopingClip(GetInstanceID());

            foreach(MeshRenderer meshRenderer in meshRenderersToHideWhenCrashing)
            {
                meshRenderer.enabled = false;
            }

            audioManager.PlayGameplayClip(ClipIds.AIRCRAFT_EXPLOSION_CLIP);
            ParticleSystem explosionParticlesInstance = Instantiate(vfxAircraftCrashed, transform.position, Quaternion.identity);
            explosionParticlesInstance.Play();
            timer = new Timer(explosionParticlesInstance.main.startLifetimeMultiplier);
            timer.OnTimerCompleted += SendAircraftDestroyedEvent;
            timer.Start();
        }

        private void SendAircraftDestroyedEvent()
        {
            timer.OnTimerCompleted -= SendAircraftDestroyedEvent;
            EventDispatcher.Instance.Dispatch(GameplayEvents.OnAircraftDestroyed, this);
        }

        private void HandleGameplayEvents(GameplayEvents gameplayEvent, object data)
        {
            switch(gameplayEvent)
            {
                case GameplayEvents.OnWallcollision:
                {
                    Collider collider = data as Collider;
                    
                    if(collider.gameObject != gameObject)
                    {
                        return;
                    }

                    DestroyAircraft();
                    break;
                }
            }
        }

        private Vector3 CalculateVelocity()
        {
            if (direction.x != 0)
            {
                horizontalSpeed = Mathf.Clamp(horizontalSpeed + acceleration * Time.deltaTime * direction.x, -movementSpeed, movementSpeed);
            }
            else
            {
                if (horizontalSpeed != 0)
                {
                    horizontalSpeed = horizontalSpeed > 0 ?
                        Mathf.Max(horizontalSpeed - acceleration * Time.deltaTime, 0) :
                        Mathf.Min(horizontalSpeed + acceleration * Time.deltaTime, 0);
                }
            }

            if (direction.y != 0)
            {
                verticalSpeed = Mathf.Clamp(verticalSpeed + acceleration * Time.deltaTime * direction.y, -movementSpeed, movementSpeed);
            }
            else
            {
                if (verticalSpeed != 0)
                {
                    verticalSpeed = verticalSpeed > 0 ?
                        Mathf.Max(verticalSpeed - acceleration * Time.deltaTime, 0) :
                        Mathf.Min(verticalSpeed + acceleration * Time.deltaTime, 0);
                }
            }

            Vector3 velocity = new Vector3(horizontalSpeed, verticalSpeed);
            //We are clamping the magnitude since going diagonally gives us a velocity magnitude that goes 
            //higher than the movement speed which, in turn, makes the movement fater than what it should be.
            //And yes, the direction also has this constraint, but the problem comes back when calculating 
            //the speed for each axis (-_-) 
            return Vector3.ClampMagnitude(velocity, movementSpeed);
        }
    }
}

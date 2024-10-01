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
        public const string AIRCRAFT_TAG = "Aircraft";
        private const string HORIZONTAL_SPEED_ANIM_VAR_NAME = "HorizontalSpeed";
        private const string VERTICAL_SPEED_ANIM_VAR_NAME = "VerticalSpeed";

        [SerializeField, Min(1)]
        private float movementSpeed = 10;

        [SerializeField, Min(1)]
        private float acceleration = 50;

        [SerializeField]
        private ParticleSystem vfxAircraftCrashed = null;

        [SerializeField]
        private MeshRenderer[] meshRenderersToHideWhenCrashing = null;

        [SerializeField]
        private Animator animator = null;

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
            UpdateAnimation(currentVelocity);
            UpdatePosition(currentVelocity);
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

        private void UpdatePosition(Vector3 currentVelocity)
        {
            Vector3 velocityOverTime = currentVelocity * Time.deltaTime;
            Vector3 newPosition = transform.position + velocityOverTime;

            transform.position = new Vector3(
                Mathf.Clamp(newPosition.x, boundaries.left, boundaries.right),
                Mathf.Clamp(newPosition.y, boundaries.bottom, boundaries.top),
                transform.position.z);
        }

        private void UpdateAnimation(Vector3 currentVelocity)
        {
            float horizontalSpeedNormalized = currentVelocity.x / movementSpeed;
            float verticalSpeedNormalized = currentVelocity.y / movementSpeed;
            animator.SetFloat(HORIZONTAL_SPEED_ANIM_VAR_NAME, horizontalSpeedNormalized);
            animator.SetFloat(VERTICAL_SPEED_ANIM_VAR_NAME, verticalSpeedNormalized);
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
            float desiredHorizontalSpeed = direction.x * movementSpeed;

            if(horizontalSpeed < desiredHorizontalSpeed)
            {
                horizontalSpeed = Mathf.Min(horizontalSpeed + acceleration * Time.deltaTime, desiredHorizontalSpeed);
            }
            else if(horizontalSpeed > desiredHorizontalSpeed)
            {
                horizontalSpeed = Mathf.Max(horizontalSpeed - acceleration * Time.deltaTime, desiredHorizontalSpeed);
            }

            float desiredVerticalSpeed = direction.y * movementSpeed;

            if (verticalSpeed < desiredVerticalSpeed)
            {
                verticalSpeed = Mathf.Min(verticalSpeed + acceleration * Time.deltaTime, desiredVerticalSpeed);
            }
            else if (verticalSpeed > desiredVerticalSpeed)
            {
                verticalSpeed = Mathf.Max(verticalSpeed - acceleration * Time.deltaTime, desiredVerticalSpeed);
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

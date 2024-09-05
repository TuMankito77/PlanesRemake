namespace PlanesRemake.Runtime.Gameplay
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Input;
    using PlanesRemake.Runtime.Utils;
    using PlanesRemake.Runtime.Events;
    using System;

    public class Aircraft : MonoBehaviour, IInputControlableEntity, IListener
    {
        //NOTE: This is momentaneous, we have to make this tag
        //changeble from a drop-down menu on each object that uses it.
        public static string AIRCRAFT_TAG = "Aircraft";

        [SerializeField, Min(1)]
        private float movementSpeed = 10;

        private Vector2 direction = Vector2.zero;
        private CameraExtensions.Boundaries boundaries = default(CameraExtensions.Boundaries);
        //NOTE: Remove this timer once we have an animation an we know when the destroy animation finishes.
        private Timer timer = null;

        #region Unity Methods

        private void Update()
        {
            if(direction.magnitude <= 0)
            {
                return;
            }

            float speedOverTime = movementSpeed * Time.deltaTime;
            Vector3 forwardSpeedChange = Vector3.right * speedOverTime * direction.x;
            Vector3 upwardSpeedChange = Vector3.up * speedOverTime * direction.y;
            Vector3 velocity = forwardSpeedChange + upwardSpeedChange;
            Vector3 newPosition = transform.position + velocity;

            transform.position = new Vector3(
                Mathf.Clamp(newPosition.x, boundaries.left, boundaries.right),
                Mathf.Clamp(newPosition.y, boundaries.bottom, boundaries.top),
                transform.position.z);
        }

        private void OnDestroy()
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

        public void Initialize(Camera sourceIsometricCamera)
        {
            boundaries = sourceIsometricCamera.GetScreenBoundariesInWorld(transform.position);
            transform.position = boundaries.center;
            EventDispatcher.Instance.AddListener(this, typeof(GameplayEvents));
        }

        public void UpdateDirection(Vector2 sourceDirection)
        {
           direction = sourceDirection.normalized;
        }

        private void DestroyAircraft()
        {
            direction = Vector2.zero;
            timer = new Timer(5);
            timer.OnTimerCompleted += SendAircraftDestroyedEvent;
            timer.Start();
        }

        private void SendAircraftDestroyedEvent()
        {
            timer.OnTimerCompleted -= SendAircraftDestroyedEvent;
            EventDispatcher.Instance.RemoveListener(this, typeof(GameplayEvents));
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
    }
}

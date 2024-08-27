namespace PlanesRemake.Runtime.Gameplay
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Input;
    using PlanesRemake.Runtime.Utils;

    public class Aircraft : MonoBehaviour, IInputControlableEntity
    {
        [SerializeField, Min(1)]
        private float movementSpeed = 10;

        private Vector2 direction = Vector2.zero;
        private Camera isometricCamera = null;
        private CameraExtensions.Boundaries boundaries = default(CameraExtensions.Boundaries);

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

        #endregion

        public void Initialize(Camera sourceIsometricCamera)
        {
            boundaries = sourceIsometricCamera.GetScreenBoundariesInWorld(transform.position);
            transform.position = boundaries.center;
        }

        public void UpdateDirection(Vector2 sourceDirection)
        {
           direction = sourceDirection.normalized;
        }
    }
}

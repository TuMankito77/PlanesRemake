namespace PlanesRemastered.Runtime.Gameplay
{
    using UnityEngine;
    
    using PlanesRemastered.Runtime.Input;

    public class Aircraft : MonoBehaviour, IInputControlableEntity
    {
        private struct Boundaries
        {
            public float top;
            public float bottom;
            public float left;
            public float right;
            public Vector3 center;
        }

        [SerializeField, Min(1)]
        private float movementSpeed = 10;

        private Vector2 direction = Vector2.zero;
        private Camera isometricCamera = null;
        private Boundaries boundaries = default(Boundaries);

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
            isometricCamera = sourceIsometricCamera;
            GetCameraBoundaries();
            transform.position = boundaries.center;
        }

        public void UpdateDirection(Vector2 sourceDirection)
        {
           direction = sourceDirection.normalized;
        }

        private void GetCameraBoundaries()
        {
            float screenHeight = isometricCamera.pixelHeight;
            float screenWidth = isometricCamera.pixelWidth;
            Vector3 aircraftToCameraVector = transform.position - isometricCamera.transform.position;

            float depthDistanceToCamera = aircraftToCameraVector.magnitude;
            boundaries.top = isometricCamera.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, depthDistanceToCamera)).y;
            boundaries.bottom = isometricCamera.ScreenToWorldPoint(new Vector3(0, 0, depthDistanceToCamera)).y;
            boundaries.left = isometricCamera.ScreenToWorldPoint(new Vector3(0, 0, depthDistanceToCamera)).x;
            boundaries.right = isometricCamera.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, depthDistanceToCamera)).x;
            boundaries.center = isometricCamera.ScreenToWorldPoint(new Vector3(screenWidth / 2, screenHeight / 2, depthDistanceToCamera));
        }
    }
}

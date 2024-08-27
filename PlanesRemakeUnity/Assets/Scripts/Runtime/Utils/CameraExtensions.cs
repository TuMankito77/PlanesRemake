namespace PlanesRemake.Runtime.Utils
{
    using UnityEngine;
    
    public static class CameraExtensions
    {
        public struct Boundaries
        {
            public float top;
            public float bottom;
            public float left;
            public float right;
            public Vector3 center;
        }

        public static Boundaries GetScreenBoundariesInWorld(this Camera camera, Vector3 worldPosition)
        {
            Boundaries boundaries = new Boundaries();
            float screenHeight = camera.pixelHeight;
            float screenWidth = camera.pixelWidth;
            Vector3 aircraftToCameraVector = worldPosition - camera.transform.position;

            float depthDistanceToCamera = aircraftToCameraVector.magnitude;
            boundaries.top = camera.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, depthDistanceToCamera)).y;
            boundaries.bottom = camera.ScreenToWorldPoint(new Vector3(0, 0, depthDistanceToCamera)).y;
            boundaries.left = camera.ScreenToWorldPoint(new Vector3(0, 0, depthDistanceToCamera)).x;
            boundaries.right = camera.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, depthDistanceToCamera)).x;
            boundaries.center = camera.ScreenToWorldPoint(new Vector3(screenWidth / 2, screenHeight / 2, depthDistanceToCamera));

            return boundaries;
        }
    }
}
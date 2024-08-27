namespace PlanesRemake.Runtime.Gameplay
{
    using UnityEngine;

    using PlanesRemake.Runtime.Utils;

    public class ObstacleSpawner
    {
        private readonly Quaternion obstacleDefaultRotation = Quaternion.Euler(new Vector3(-15, -15, 0));
        
        private Obstacle obstaclePrefab = null;
        private Timer spawningTimer = null;
        private CameraExtensions.Boundaries boundaries = default(CameraExtensions.Boundaries);
        private Vector3 startingPosition = Vector3.zero;

        public ObstacleSpawner(Obstacle sourceObstaclePrefab, Camera sourceIsometricCamera)
        {
            obstaclePrefab = sourceObstaclePrefab;
            
            boundaries = sourceIsometricCamera.GetScreenBoundariesInWorld(Vector3.zero);
            startingPosition = new Vector3(
                boundaries.right,
                boundaries.center.y,
                0);

            spawningTimer = new Timer(10, sourceIsRepeating: true);
            spawningTimer.OnTimerCompleted += OnSpawningTimerCompleted;
            spawningTimer.Start();
        }

        private void SpawnObstacle(Obstacle obstaclePrefab)
        {
            Obstacle newObstacle = GameObject.Instantiate(obstaclePrefab);
            newObstacle.Initialize(1, 10, boundaries.top, startingPosition, obstacleDefaultRotation);
        }

        private void OnSpawningTimerCompleted()
        {
            SpawnObstacle(obstaclePrefab);
        }
    }
}
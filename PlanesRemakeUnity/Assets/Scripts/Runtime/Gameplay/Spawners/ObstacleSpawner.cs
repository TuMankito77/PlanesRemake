namespace PlanesRemake.Runtime.Gameplay.Spawners
{
    using UnityEngine;

    public class ObstacleSpawner : TimerSpawner
    {
        protected override Vector3 StartingPosition => 
            new Vector3(boundaries.right, boundaries.center.y, 0);
        protected override Quaternion StartingRotation =>
            Quaternion.Euler(new Vector3(-15, -15, 0));
        protected override float SpawnDelayInSeconds => 30;
        protected override bool SpawnPrefabOnCreation => true;

        public ObstacleSpawner(Obstacle obstaclePrefab, int obstaclePoolSize, int obstaclePoolMaxCapacity, Camera isometricCamera)
            :base(obstaclePrefab, obstaclePoolSize, obstaclePoolMaxCapacity, isometricCamera)
        {

        }

        protected override void OnGetPoolObject(BasePoolableObject instance)
        {
            base.OnGetPoolObject(instance);
            Obstacle obstacle = instance.GetComponent<Obstacle>();
            obstacle.Initialize(Aircraft.AIRCRAFT_TAG, prefabInstancesPool, boundaries, 1, 10, boundaries.top, StartingPosition, StartingRotation);
        }
    }
}
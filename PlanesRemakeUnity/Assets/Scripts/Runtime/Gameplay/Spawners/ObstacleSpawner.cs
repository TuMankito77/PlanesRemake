namespace PlanesRemake.Runtime.Gameplay.Spawners
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Utils;

    public class ObstacleSpawner : TimerSpawner
    {
        protected override Vector3 StartingPosition => 
            new Vector3(boundaries.right, boundaries.center.y, 0);
        protected override Quaternion StartingRotation =>
            Quaternion.Euler(new Vector3(-15, 15, 0));
        protected override float SpawnDelayInSeconds => GetRandomSpawningTime();
        protected override bool SpawnPrefabOnCreation => true;

        public ObstacleSpawner(Obstacle obstaclePrefab, int obstaclePoolSize, int obstaclePoolMaxCapacity, Camera isometricCamera, CameraBoundaries cameraBoundariesOffset)
            :base(obstaclePrefab, obstaclePoolSize, obstaclePoolMaxCapacity, isometricCamera, cameraBoundariesOffset)
        {

        }

        protected override void OnGetPoolObject(BasePoolableObject instance)
        {
            base.OnGetPoolObject(instance);
            Obstacle obstacle = instance as Obstacle;
            int osilatingSpeed = Random.Range(5, 11);
            float osilationDistance = Random.Range(1, boundaries.top);
            obstacle.Initialize(Aircraft.AIRCRAFT_TAG, prefabInstancesPool, boundaries, 1, osilatingSpeed, osilationDistance);
        }

        private int GetRandomSpawningTime()
        {
            return Random.Range(15 , 26);
        }
    }
}
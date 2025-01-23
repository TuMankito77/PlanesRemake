namespace PlanesRemake.Runtime.Gameplay.Spawners
{
    using System.Collections.Generic;
    
    using UnityEngine;

    using PlanesRemake.Runtime.Utils;
    using PlanesRemake.Runtime.Events;

    public class ObstacleSpawner : TimerSpawner
    {
        protected override Vector3 StartingPosition =>
            new Vector3(boundaries.right, boundaries.center.y, 0);
        protected override Quaternion StartingRotation =>
            Quaternion.Euler(new Vector3(-15, 15, 0));
        
        private bool spawnPrefabOnCreation => true;

        public ObstacleSpawner(
            Obstacle obstaclePrefab,
            int obstaclePoolSize,
            int obstaclePoolMaxCapacity,
            Camera isometricCamera,
            CameraBoundaries cameraBoundariesOffset,
            int sourceMinSpawningTime,
            int sourceMaxSpawningTime)
            : base(
                 obstaclePrefab,
                 obstaclePoolSize,
                 obstaclePoolMaxCapacity,
                 isometricCamera,
                 cameraBoundariesOffset,
                 getSpawningDelayInSeconds: () => { return Random.Range(sourceMinSpawningTime, sourceMaxSpawningTime); })
        {
            if(spawnPrefabOnCreation)
            {
                prefabInstancesPool.Get();
            }
        }

        protected override void OnGetPoolObject(BasePoolableObject instance)
        {
            base.OnGetPoolObject(instance);
            Obstacle obstacle = instance as Obstacle;
            int osilatingSpeed = Random.Range(5, 11);
            float osilationDistance = Random.Range(1, boundaries.top);
            obstacle.Initialize(Aircraft.CRASH_DETECTION_COLLIDER_TAG, Aircraft.AIRCRAFT_TAG, prefabInstancesPool, boundaries, movementSpeed: 3, osilatingSpeed, osilationDistance);
        }
    }
}
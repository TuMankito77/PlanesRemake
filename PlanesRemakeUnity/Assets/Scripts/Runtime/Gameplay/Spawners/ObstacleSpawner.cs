namespace PlanesRemake.Runtime.Gameplay.Spawners
{
    using UnityEngine;

    public class ObstacleSpawner : BaseSpawner
    {
        protected override Vector3 StartingPosition => 
            new Vector3(boundaries.right, boundaries.center.y, 0);
        protected override Quaternion StartingRotation =>
            Quaternion.Euler(new Vector3(-15, -15, 0));
        protected override float SpawnDelayInSeconds => 30;
        protected override bool SpawnPrefabOnCreation => true;

        public ObstacleSpawner(GameObject obstaclePrefab, Camera isometricCamera)
            :base(obstaclePrefab, isometricCamera)
        {

        }

        protected override GameObject SpawnPrefab(GameObject prefab)
        {
            GameObject obstacleGameObject = base.SpawnPrefab(prefab);
            Obstacle newObstacle = obstacleGameObject.GetComponent<Obstacle>();
            newObstacle.Initialize("Aircraft", 1, 10, boundaries.top, StartingPosition, StartingRotation);
            return obstacleGameObject;
        }
    }
}
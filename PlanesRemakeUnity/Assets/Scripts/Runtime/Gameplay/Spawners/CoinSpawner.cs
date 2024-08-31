namespace PlanesRemake.Runtime.Gameplay.Spawners
{
    using UnityEngine;
    
    public class CoinSpawner : BaseSpawner
    {
        protected override Vector3 StartingPosition => GetRandomHeightPosition();
        protected override Quaternion StartingRotation => Quaternion.identity;
        protected override float SpawnDelayInSeconds => 5;
        protected override bool SpawnPrefabOnCreation => false;
    
        public CoinSpawner(GameObject coinPrefab, Camera isometricCamera) 
            : base(coinPrefab, isometricCamera)
        {
            
        }

        private Vector3 GetRandomHeightPosition()
        {
            float randomYPosition = Random.Range(boundaries.bottom, boundaries.top);
            return new Vector3(boundaries.right, randomYPosition, 0);
        }
    }
}

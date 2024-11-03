namespace PlanesRemake.Runtime.Gameplay.Spawners
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Sound;
    using PlanesRemake.Runtime.Utils;
    
    public class CoinSpawner : TimerSpawner
    {
        protected override Vector3 StartingPosition => GetRandomHeightPosition();
        protected override Quaternion StartingRotation => Quaternion.Euler(new Vector3(-15, 15, 0));
        protected override float SpawnDelayInSeconds => 5;
        protected override bool SpawnPrefabOnCreation => false;

        private AudioManager audioManager = null;
    
        public CoinSpawner(Coin coinPrefab, int coinPoolSize, int coinPoolMaxCapacity, Camera isometricCamera, AudioManager sourceAudioManager, CameraBoundaries cameraBoundariesOffset) 
            : base(coinPrefab, coinPoolSize, coinPoolMaxCapacity, isometricCamera, cameraBoundariesOffset)
        {
            audioManager = sourceAudioManager;
        }

        private Vector3 GetRandomHeightPosition()
        {
            float randomYPosition = Random.Range(boundaries.bottom, boundaries.top);
            return new Vector3(boundaries.right, randomYPosition, 0);
        }

        protected override void OnGetPoolObject(BasePoolableObject instance)
        {
            base.OnGetPoolObject(instance);
            Coin coin = instance as Coin;
            coin.Initialize(Aircraft.AIRCRAFT_TAG, prefabInstancesPool, boundaries, audioManager);
        }
    }
}

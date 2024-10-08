namespace PlanesRemake.Runtime.Gameplay.Spawners
{
    using PlanesRemake.Runtime.Sound;
    using UnityEngine;
    
    public class CoinSpawner : TimerSpawner
    {
        protected override Vector3 StartingPosition => GetRandomHeightPosition();
        protected override Quaternion StartingRotation => Quaternion.identity;
        protected override float SpawnDelayInSeconds => 5;
        protected override bool SpawnPrefabOnCreation => false;

        private AudioManager audioManager = null;
    
        public CoinSpawner(Coin coinPrefab, int coinPoolSize, int coinPoolMaxCapacity, Camera isometricCamera, AudioManager sourceAudioManager) 
            : base(coinPrefab, coinPoolSize, coinPoolMaxCapacity, isometricCamera)
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
            Coin coin = instance.GetComponent<Coin>();
            coin.Initialize(Aircraft.AIRCRAFT_TAG, prefabInstancesPool, boundaries, audioManager);
        }
    }
}

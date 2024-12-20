namespace PlanesRemake.Runtime.Gameplay.Spawners
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Sound;
    using PlanesRemake.Runtime.Utils;
    
    public class PickUpSpawner : TimerSpawner
    {
        private int minSpawningTime = 0;
        private int maxSpawningTime = 0;

        protected override Vector3 StartingPosition => GetRandomHeightPosition();
        protected override Quaternion StartingRotation => Quaternion.Euler(new Vector3(-15, 15, 0));
        protected override float SpawnDelayInSeconds => GetSpawningTime();
        protected override bool SpawnPrefabOnCreation => false;

        private AudioManager audioManager = null;
    
        public PickUpSpawner(
            BasePickUpItem pickUpItemPrefab, 
            int pickUpItemPoolSize, 
            int pickUpItemPoolMaxCapacity, 
            Camera isometricCamera, 
            AudioManager sourceAudioManager, 
            CameraBoundaries cameraBoundariesOffset,
            int sourceMinSpawningTime,
            int sourceMaxSpawningTime) 
            : base(pickUpItemPrefab, pickUpItemPoolSize, pickUpItemPoolMaxCapacity, isometricCamera, cameraBoundariesOffset)
        {
            audioManager = sourceAudioManager;
            minSpawningTime = sourceMinSpawningTime;
            maxSpawningTime = sourceMaxSpawningTime;
        }

        private Vector3 GetRandomHeightPosition()
        {
            float randomYPosition = Random.Range(boundaries.bottom, boundaries.top);
            return new Vector3(boundaries.right, randomYPosition, 0);
        }

        protected override void OnGetPoolObject(BasePoolableObject instance)
        {
            base.OnGetPoolObject(instance);
            BasePickUpItem pickUpItem = instance as BasePickUpItem;
            pickUpItem.Initialize(Aircraft.AIRCRAFT_TAG, prefabInstancesPool, boundaries, audioManager);
        }

        protected virtual int GetSpawningTime()
        {
            return Random.Range(minSpawningTime, maxSpawningTime);
        }
    }
}

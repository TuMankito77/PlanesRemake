namespace PlanesRemake.Runtime.Core
{
    using System.Collections.Generic;
    
    using UnityEngine;

    using PlanesRemake.Runtime.Gameplay;
    using PlanesRemake.Runtime.Input;
    using PlanesRemake.Runtime.Gameplay.Spawners;
    using PlanesRemake.Runtime.Sound;
    using PlanesRemake.Runtime.UI;
    using PlanesRemake.Runtime.UI.Views;
    using System.Threading.Tasks;

    public class LevelInitializer : BaseSystem
    {
        private const string SPHERICAL_BACKGROUND_PREFAB_PATH = "MainLevel/SphericalBackground";
        private const string AIRCRAFT_PREFAB_PATH = "MainLevel/Aircraft";
        private const string OBSTACLE_PREFAB_PATH = "MainLevel/Obstacle";
        private const string COIN_PREFAB_PATH = "MainLevel/Coin";
        private const string COIN_VFX_PREFAB_PATH = "MainLevel/VFX_CoinCollected";
        private const int SPAWNER_POOL_SIZE = 10;
        private const int SPAWNER_POOL_MAX_CAPACITY = 100;

        private GameObject skyDomeBackground = null;
        private List<BaseSpawner> spawners = null;
        private Aircraft aircraft = null;
        private ContentLoader contentLoader = null;
        private AudioManager audioManager = null;
        private Camera isometricCamera = null;

        public Aircraft Aircraft => aircraft;

        public LevelInitializer(ContentLoader sourceContentLoader, AudioManager sourceAudioManager)
        {
            spawners = new List<BaseSpawner>();
            contentLoader = sourceContentLoader;
            audioManager = sourceAudioManager;
            //NOTE: Update this so that we do not look for this object by name, but rather by reference 
            //as it could cause performance issues. 
            isometricCamera = GameObject.Find("IsometricCamera").GetComponent<Camera>();
        }

        public override async Task<bool> Initialize(IEnumerable<BaseSystem> sourceDependencies)
        {
            await base.Initialize(sourceDependencies);

            //To-do: Use the assets loaded here to grab the information about the players choice for the background and the aircraft chosen.
            GameObject skyDomeBackgroundPrefab = await contentLoader.LoadAsset<GameObject>(SPHERICAL_BACKGROUND_PREFAB_PATH);
            skyDomeBackground = GameObject.Instantiate(skyDomeBackgroundPrefab, Vector3.zero, Quaternion.identity);
            Aircraft aircraftPrefab = await contentLoader.LoadAsset<Aircraft>(AIRCRAFT_PREFAB_PATH);
            aircraft = GameObject.Instantiate(aircraftPrefab, Vector3.zero, Quaternion.Euler(0, 115, -25));
            aircraft.Initialize(isometricCamera, audioManager);
            Obstacle obstaclePrefab = await contentLoader.LoadAsset<Obstacle>(OBSTACLE_PREFAB_PATH);
            spawners.Add(new ObstacleSpawner(obstaclePrefab, SPAWNER_POOL_SIZE, SPAWNER_POOL_MAX_CAPACITY, isometricCamera));
            Coin coinPrefab = await contentLoader.LoadAsset<Coin>(COIN_PREFAB_PATH);
            spawners.Add(new CoinSpawner(coinPrefab, SPAWNER_POOL_SIZE, SPAWNER_POOL_MAX_CAPACITY, isometricCamera, audioManager));
            TimerPoolableObject timerPoolableObjectPrefab = await contentLoader.LoadAsset<TimerPoolableObject>(COIN_VFX_PREFAB_PATH);
            spawners.Add(new CoinParticleSpawner(timerPoolableObjectPrefab, SPAWNER_POOL_SIZE, SPAWNER_POOL_MAX_CAPACITY));
            return true;
        }

        public void Dispose()
        {
            foreach (BaseSpawner spawner in spawners)
            {
                spawner.Dispose();
            }

            aircraft.Dispose();
        }
    }
}
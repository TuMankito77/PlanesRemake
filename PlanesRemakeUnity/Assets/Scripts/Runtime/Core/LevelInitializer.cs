namespace PlanesRemake.Runtime.Core
{
    using System.Collections.Generic;
    
    using UnityEngine;

    using PlanesRemake.Runtime.Gameplay;
    using PlanesRemake.Runtime.Input;
    using PlanesRemake.Runtime.Gameplay.Spawners;
    using PlanesRemake.Runtime.Sound;

    //NOTE: Should we make this a system?
    //Probably yes, as it loads data asynchronously and sometimes we need to know when we have access to this data.
    public class LevelInitializer
    {
        private const string SPHERICAL_BACKGROUND_PREFAB_PATH = "MainLevel/SphericalBackground";
        private const string AIRCRAFT_PREFAB_PATH = "MainLevel/Aircraft";
        private const string OBSTACLE_PREFAB_PATH = "MainLevel/Obstacle";
        private const string COIN_PREFAB_PATH = "MainLevel/Coin";
        private const string COIN_VFX_PREFAB_PATH = "MainLevel/VFX_CoinCollected";
        private const string JOYSTICK_INPUT_PREFAB_PATH = "TouchControls/JoystickInput";
        private const int SPAWNER_POOL_SIZE = 10;
        private const int SPAWNER_POOL_MAX_CAPACITY = 100;

        private GameObject skyDomeBackground = null;
        private List<BaseSpawner> spawners = null;
        private Aircraft aircraft = null;

        public Aircraft Aircraft => aircraft;

        public LevelInitializer(ContentLoader contentLoader, InputManager inputManager, AudioManager audioManager)
        {
            spawners = new List<BaseSpawner>();
            //NOTE: Update this so that we do not look for this object by name, but rather by reference 
            //as it could cause performance issues. 
            Camera isometricCamera = GameObject.Find("IsometricCamera").GetComponent<Camera>();
            //To-do: Use the assets loaded here to grab the information about the players choice for the background and the aircraft chosen.
            contentLoader.LoadAsset<GameObject>
                (SPHERICAL_BACKGROUND_PREFAB_PATH,
                (assetLoaded) => skyDomeBackground = GameObject.Instantiate(assetLoaded, Vector3.zero, Quaternion.identity),
                null);

            contentLoader.LoadAsset<Aircraft>
                (AIRCRAFT_PREFAB_PATH,
                (assetLoaded) =>
                {
                    aircraft = GameObject.Instantiate(assetLoaded, Vector3.zero, Quaternion.Euler(0, 115, -25));
                    aircraft.Initialize(isometricCamera, audioManager);
                    inputManager.EnableInput(Aircraft);
                },
                null);

            contentLoader.LoadAsset<Obstacle>
                (OBSTACLE_PREFAB_PATH,
                (assetLoaded) => spawners.Add(new ObstacleSpawner(assetLoaded, SPAWNER_POOL_SIZE, SPAWNER_POOL_MAX_CAPACITY, isometricCamera)),
                null);

            contentLoader.LoadAsset<Coin>
                (COIN_PREFAB_PATH,
                (assetLoaded) => spawners.Add(new CoinSpawner(assetLoaded, SPAWNER_POOL_SIZE, SPAWNER_POOL_MAX_CAPACITY, isometricCamera, audioManager)),
                null);

            contentLoader.LoadAsset<TimerPoolableObject>
                (COIN_VFX_PREFAB_PATH,
                (assetLoaded) => spawners.Add(new CoinParticleSpawner(assetLoaded, SPAWNER_POOL_SIZE, SPAWNER_POOL_MAX_CAPACITY)),
                null);

            audioManager.PlayBackgroundMusic(ClipIds.MUSIC_CLIP);
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
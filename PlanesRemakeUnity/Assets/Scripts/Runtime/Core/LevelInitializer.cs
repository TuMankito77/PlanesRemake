namespace PlanesRemake.Runtime.Core
{
    using UnityEngine;

    using PlanesRemake.Runtime.Gameplay;
    using PlanesRemake.Runtime.Input;
    using PlanesRemake.Runtime.Gameplay.Spawners;
    using System.Collections.Generic;

    //NOTE: Should we make this a system?
    //Probably yes, as it loads data asynchronously and sometimes we need to know when we have access to this data.
    public class LevelInitializer
    {
        private const string SPHERICAL_BACKGROUND_PREFAB_PATH = "MainLevel/SphericalBackground";
        private const string AIRCRAFT_PREFAB_PATH = "MainLevel/Aircraft";
        private const string OBSTACLE_PREFAB_PATH = "MainLevel/Obstacle";
        private const string COIN_PREFAB_PATH = "MainLevel/Coin";
        private const int SPAWNER_POOL_SIZE = 10;
        private const int SPAWNER_POOL_MAX_CAPACITY = 100;

        private GameObject skyDomeBackground = null;
        private List<BaseSpawner> spawners = null;

        public Aircraft Aircraft { get; private set; } = null;

        public LevelInitializer(ContentLoader contentLoader, InputManager inputManager)
        {
            spawners = new List<BaseSpawner>();
            //NOTE: Update this so that we do not look for this object by name, but rather by reference 
            //as it could cause performance issues. 
            Camera isometricCamera = GameObject.Find("IsometricCamera").GetComponent<Camera>();
            //To-do: Use the assets loaded here to grab the information about the players choice for the background and the aircraft chosen.
            contentLoader.LoadAsset<GameObject>
                (SPHERICAL_BACKGROUND_PREFAB_PATH,
                (assetLoaded) => skyDomeBackground = GameObject.Instantiate(assetLoaded, Vector3.zero, Quaternion.identity),
                () => DisplayAssetNotLoadedError(SPHERICAL_BACKGROUND_PREFAB_PATH));

            contentLoader.LoadAsset<Aircraft>
                (AIRCRAFT_PREFAB_PATH,
                (assetLoaded) =>
                {
                    Aircraft = GameObject.Instantiate(assetLoaded, Vector3.zero, Quaternion.Euler(0, 115, -25));
                    Aircraft.Initialize(isometricCamera);
                    inputManager.EnableInput(Aircraft);
                },
                () => DisplayAssetNotLoadedError(AIRCRAFT_PREFAB_PATH));

            contentLoader.LoadAsset<Obstacle>
                (OBSTACLE_PREFAB_PATH,
                (assetLoaded) => spawners.Add(new ObstacleSpawner(assetLoaded, SPAWNER_POOL_SIZE, SPAWNER_POOL_MAX_CAPACITY, isometricCamera)),
                () => DisplayAssetNotLoadedError(OBSTACLE_PREFAB_PATH));

            contentLoader.LoadAsset<Coin>
                (COIN_PREFAB_PATH,
                (assetLoaded) => spawners.Add(new CoinSpawner(assetLoaded, SPAWNER_POOL_SIZE, SPAWNER_POOL_MAX_CAPACITY, isometricCamera)),
                () => DisplayAssetNotLoadedError(COIN_PREFAB_PATH));
        }

        public void Dispose()
        {
            foreach (BaseSpawner spawner in spawners)
            {
                spawner.Dispose();
            }
        }

        //To-do: Consider moving this function to the ContentLoader class.
        private void DisplayAssetNotLoadedError(string assetAddress)
        {
            Debug.LogError($"Failed to load asset with address {assetAddress}");
        }
    }
}
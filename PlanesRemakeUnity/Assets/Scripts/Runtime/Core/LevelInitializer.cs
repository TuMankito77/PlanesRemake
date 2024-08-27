namespace PlanesRemake.Runtime.Core
{
    using PlanesRemake.Runtime.Gameplay;
    using PlanesRemake.Runtime.Input;
    using UnityEngine;

    //NOTE: Should we make this a system?
    //Probably yes, as it loads data asynchronously and sometimes we need to know when we have access to this data.
    public class LevelInitializer
    {
        private const string SPHERICAL_BACKGROUND_PREFAB_PATH = "MainLevel/SphericalBackground";
        private const string PLAYER_PLANE_PREFAB_PATH = "MainLevel/Player";
        private const string OBSTACLE_PREFAB_PATH = "MainLevel/Obstacle";

        private GameObject skyDomeBackground = null;
        private Aircraft playerPlane = null;
        private ObstacleSpawner obstacleSpawner = null;

        public LevelInitializer(ContentLoader contentLoader, InputManager inputManager)
        {
            //NOTE: Update this so that we do not look for this object by name, but rather by reference 
            //as it could cause performance issues. 
            Camera isometricCamera = GameObject.Find("IsometricCamera").GetComponent<Camera>();
            //To-do: Use the assets loaded here to grab the information about the players choice for the background and the aircraft chosen.
            contentLoader.LoadAsset<GameObject>
                (SPHERICAL_BACKGROUND_PREFAB_PATH, 
                (assetLoaded) => skyDomeBackground = GameObject.Instantiate(assetLoaded, Vector3.zero, Quaternion.identity), 
                () => DisplayAssetNotLoadedError(SPHERICAL_BACKGROUND_PREFAB_PATH));

            contentLoader.LoadAsset<Aircraft>
                (PLAYER_PLANE_PREFAB_PATH, 
                (assetLoaded) =>
                {
                    playerPlane = GameObject.Instantiate(assetLoaded, Vector3.zero, Quaternion.Euler(0, 115, -25));
                    playerPlane.Initialize(isometricCamera);
                    inputManager.EnableInput(playerPlane);
                },
                () => DisplayAssetNotLoadedError(PLAYER_PLANE_PREFAB_PATH));

            contentLoader.LoadAsset<Obstacle>
                (OBSTACLE_PREFAB_PATH,
                (assetLoaded) => obstacleSpawner = new ObstacleSpawner(assetLoaded, isometricCamera),
                ()=> DisplayAssetNotLoadedError(OBSTACLE_PREFAB_PATH));
        }

        //Investigate how to call the destructor of a class in order to clean-up everything that it was managing.
        ~LevelInitializer()
        {
            //Dispose everything that wee need to dispose.
        }

        //To-do: Consider moving this function to the ContentLoader class.
        private void DisplayAssetNotLoadedError(string assetAddress)
        {
            Debug.LogError($"Failed to load asset with address {assetAddress}");
        }
    }
}
namespace PlanesRemastered.Runtime.Core
{
    using PlanesRemastered.Runtime.Gameplay;
    using PlanesRemastered.Runtime.Input;
    using UnityEngine;

    public class LevelInitializer
    {
        private const string SPHERICAL_BACKGROUND_PREFAB_PATH = "MainLevel/SphericalBackground";
        private const string PLAYER_PLANE_PREFAB_PATH = "MainLevel/Player";

        private GameObject skyDomeBackground = null;
        private Aircraft playerPlane = null;

        public LevelInitializer(ContentLoader contentLoader, InputManager inputManager)
        {
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
                    Camera isometricCamera = GameObject.Find("IsometricCamera").GetComponent<Camera>();
                    playerPlane.Initialize(isometricCamera);
                    inputManager.EnableInput(playerPlane);
                },
                () => DisplayAssetNotLoadedError(PLAYER_PLANE_PREFAB_PATH));
        }

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
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

    //NOTE: Should we make this a system?
    //Probably yes, as it loads data asynchronously and sometimes we need to know when we have access to this data.
    public class LevelInitializer
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
        private GameplayController gameplayContoller = null;
        private TouchCotrolsView touchControlsView = null;
        private UiManager uiManager = null;

        public Aircraft Aircraft => aircraft;

        public LevelInitializer(ContentLoader contentLoader, InputManager inputManager, AudioManager audioManager, UiManager sourceUiManager)
        {
            spawners = new List<BaseSpawner>();
            uiManager = sourceUiManager;
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
                    gameplayContoller = inputManager.EnableInput(Aircraft) as GameplayController;
                    
                    if(gameplayContoller.VirtualJoystickEnabled)
                    {
                        touchControlsView = uiManager.DisplayView(ViewIds.TOUCH_CONTROLS) as TouchCotrolsView;
                        gameplayContoller.VirtualJoystick.OnTouchStart += touchControlsView.SetInitialPosition;
                        gameplayContoller.VirtualJoystick.OnTouchDrag += touchControlsView.SetDragPosition;
                        gameplayContoller.VirtualJoystick.OnTouchEnd += touchControlsView.SetEndPosition;
                    }
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
            if (gameplayContoller.VirtualJoystickEnabled)
            {
                gameplayContoller.VirtualJoystick.OnTouchStart -= touchControlsView.SetInitialPosition;
                gameplayContoller.VirtualJoystick.OnTouchDrag -= touchControlsView.SetDragPosition;
                gameplayContoller.VirtualJoystick.OnTouchEnd -= touchControlsView.SetEndPosition;
                uiManager.RemoveView(ViewIds.TOUCH_CONTROLS);
            }

            foreach (BaseSpawner spawner in spawners)
            {
                spawner.Dispose();
            }

            aircraft.Dispose();
        }
    }
}
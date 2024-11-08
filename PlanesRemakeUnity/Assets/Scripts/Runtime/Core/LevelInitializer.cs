namespace PlanesRemake.Runtime.Core
{
    using System.Collections.Generic;
    
    using UnityEngine;

    using PlanesRemake.Runtime.Gameplay;
    using PlanesRemake.Runtime.Gameplay.Spawners;
    using PlanesRemake.Runtime.Sound;
    using System.Threading.Tasks;
    using PlanesRemake.Runtime.Utils;
    using PlanesRemake.Runtime.UI;
    using PlanesRemake.Runtime.UI.Views;

    public class LevelInitializer : BaseSystem
    {
        private const string SPHERICAL_BACKGROUND_PREFAB_PATH = "MainLevel/SphericalBackground";
        private const string AIRCRAFT_PREFAB_PATH = "MainLevel/Aircraft";
        private const string OBSTACLE_PREFAB_PATH = "MainLevel/Obstacle";
        private const string COIN_PREFAB_PATH = "MainLevel/Coin";
        private const string COIN_VFX_PREFAB_PATH = "MainLevel/VFX_CoinCollected";
        private const string FUEL_PREFAB_PATH = "MainLevel/Fuel";
        private const string BACKGROUND_RENDERING_CAMERA_PREFAB_PATH = "MainLevel/BackgroundRenderingCamera";
        private const string ISOMETRIC_CAMERA_PREFAB_PATH = "MainLevel/IsometricCamera";
        private const int SPAWNER_POOL_SIZE = 10;
        private const int SPAWNER_POOL_MAX_CAPACITY = 100;

        private GameObject skyDomeBackground = null;
        private List<BaseSpawner> spawners = null;
        private Aircraft aircraft = null;
        private UiManager uiManager = null;
        private ContentLoader contentLoader = null;
        private AudioManager audioManager = null;
        private Camera isometricCamera = null;
        private Camera backgroundRenderingCamera = null;
        private CameraStackingManager cameraStackingManager = null;

        public Aircraft Aircraft => aircraft;

        public LevelInitializer(UiManager sourceUiManager, ContentLoader sourceContentLoader, AudioManager sourceAudioManager, CameraStackingManager sourceCameraStackingManager)
        {
            spawners = new List<BaseSpawner>();
            uiManager = sourceUiManager;
            contentLoader = sourceContentLoader;
            audioManager = sourceAudioManager;
            cameraStackingManager = sourceCameraStackingManager;
        }

        public override async Task<bool> Initialize(IEnumerable<BaseSystem> sourceDependencies)
        {
            await base.Initialize(sourceDependencies);
            uiManager.DisplayView(ViewIds.HUD);
            Camera isometricCameraPrefab = await contentLoader.LoadAsset<Camera>(ISOMETRIC_CAMERA_PREFAB_PATH);
            isometricCamera = GameObject.Instantiate(isometricCameraPrefab);
            Camera backgroundRenderingCameraPrefab = await contentLoader.LoadAsset<Camera>(BACKGROUND_RENDERING_CAMERA_PREFAB_PATH);
            backgroundRenderingCamera = GameObject.Instantiate(backgroundRenderingCameraPrefab);
            cameraStackingManager.AddCameraToStackAtBottom(isometricCamera, backgroundRenderingCamera);

            //To-do: Use the assets loaded here to grab the information about the players choice for the background and the aircraft chosen.
            GameObject skyDomeBackgroundPrefab = await contentLoader.LoadAsset<GameObject>(SPHERICAL_BACKGROUND_PREFAB_PATH);
            skyDomeBackground = GameObject.Instantiate(skyDomeBackgroundPrefab, Vector3.zero, Quaternion.identity);

            CameraBoundaries aircraftCameraBoundariesOffset = new CameraBoundaries()
            {
                top = -2,
                bottom = 0,
                right = -2,
                left = 4,
                center = Vector3.zero
            };
            
            Aircraft aircraftPrefab = await contentLoader.LoadAsset<Aircraft>(AIRCRAFT_PREFAB_PATH);
            aircraft = GameObject.Instantiate(aircraftPrefab, Vector3.zero, Quaternion.Euler(15, 105, 0));
            aircraft.Initialize(isometricCamera, aircraftCameraBoundariesOffset, audioManager, fuelDuration: 50);

            //TO-DO: Move this to a scriptable object so that it can be configured from Unity rather than in code.
            CameraBoundaries cameraBoundariesOffset = new CameraBoundaries()
            {
                top = 0,
                bottom = 0,
                right = 5,
                left = -5,
                center = Vector3.zero
            };

            Obstacle obstaclePrefab = await contentLoader.LoadAsset<Obstacle>(OBSTACLE_PREFAB_PATH);
            spawners.Add(new ObstacleSpawner(obstaclePrefab, SPAWNER_POOL_SIZE, SPAWNER_POOL_MAX_CAPACITY, isometricCamera, cameraBoundariesOffset));
            Coin coinPrefab = await contentLoader.LoadAsset<Coin>(COIN_PREFAB_PATH);
            spawners.Add(new PickUpSpawner(
                coinPrefab, 
                SPAWNER_POOL_SIZE, 
                SPAWNER_POOL_MAX_CAPACITY, 
                isometricCamera, 
                audioManager, 
                cameraBoundariesOffset,
                sourceMinSpawningTime: 1,
                sourceMaxSpawningTime: 5));
            Fuel fuelPrefab = await contentLoader.LoadAsset<Fuel>(FUEL_PREFAB_PATH);
            spawners.Add(new PickUpSpawner(
                fuelPrefab, 
                SPAWNER_POOL_SIZE, 
                SPAWNER_POOL_MAX_CAPACITY, 
                isometricCamera, 
                audioManager, 
                cameraBoundariesOffset,
                sourceMinSpawningTime: 30,
                sourceMaxSpawningTime: 41));
            TimerPoolableObject timerPoolableObjectPrefab = await contentLoader.LoadAsset<TimerPoolableObject>(COIN_VFX_PREFAB_PATH);
            spawners.Add(new CoinParticleSpawner(timerPoolableObjectPrefab, SPAWNER_POOL_SIZE, SPAWNER_POOL_MAX_CAPACITY));
            return true;
        }

        public void Dispose()
        {
            uiManager.RemoveView(ViewIds.HUD);
            cameraStackingManager.RemoveCameraFromStack(isometricCamera, backgroundRenderingCamera);

            foreach (BaseSpawner spawner in spawners)
            {
                spawner.Dispose();
            }

            aircraft.Dispose();
        }
    }
}
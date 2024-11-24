namespace PlanesRemake.Runtime.Core
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.SceneManagement;

    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Gameplay;
    using PlanesRemake.Runtime.Input;
    using PlanesRemake.Runtime.UI;
    using PlanesRemake.Runtime.UI.Views;
    using PlanesRemake.Runtime.Sound;
    using PlanesRemake.Runtime.Gameplay.StorableClasses;
    using PlanesRemake.Runtime.SaveTool;
    using PlanesRemake.Runtime.Utils;
    using PlanesRemake.Runtime.UI.Views.DataContainers;
    using PlanesRemake.Runtime.Localization;

    public class GameManager : IListener
    {
        private SystemsInitializer systemsInitializer = null;
        private SystemsInitializer mainLevelSystemsInitializer = null;
        private List<BaseSystem> baseSystems = null;
        private List<BaseSystem> mainLevelSystems = null;
        private ContentLoader contentLoader = null;
        private UiManager uiManager = null;
        private AudioManager audioManager = null;
        private InputManager inputManager = null;
        private LocalizationManager localizationManager = null;
        private CameraStackingManager cameraStackingManager = null;
        private LevelInitializer currentLevelInitializer = null;
        private PlayerInformation playerInformation = null;
        private StorageAccessor storageAccessor = null;
        private int gameSessionCoinsCollected = 0;
        private int gameSessionWallsEvaded = 0;

        public bool IsGamePaused { get; private set; } = false;

        public GameManager()
        {
            Application.targetFrameRate = 60;

            baseSystems = new List<BaseSystem>();
            baseSystems.Add(new CameraStackingManager());
            baseSystems.Add(new ContentLoader());
            baseSystems.Add(new UiManager()
                .AddDependency<ContentLoader>()
                .AddDependency<AudioManager>()
                .AddDependency<CameraStackingManager>()
                .AddDependency<LocalizationManager>());
            baseSystems.Add(new AudioManager()
                .AddDependency<ContentLoader>());
            baseSystems.Add(new LocalizationManager()
                .AddDependency<ContentLoader>());
            systemsInitializer = new SystemsInitializer();
            systemsInitializer.OnSystemsInitialized += OnSystemsInitialized;
            systemsInitializer.InitializeSystems(baseSystems);

            EventDispatcher.Instance.AddListener(this, typeof(UiEvents), typeof(GameplayEvents));
        }

        ~GameManager()
        {
            systemsInitializer.OnSystemsInitialized -= OnSystemsInitialized;
        }

        #region IListener

        public void HandleEvent(IComparable eventName, object data)
        {
            switch (eventName)
            {
                case UiEvents uiEvent:
                    {
                        HandleUiEvents(uiEvent, data);
                        break;
                    }

                case GameplayEvents gameplayEvent:
                    {
                        HandleGameplayEvents(gameplayEvent, data);
                        break;
                    }

                default:
                    {
                        LoggerUtil.LogError($"{GetType()} - The event {eventName} is not handled by this class. You may need to unsubscribe.");
                        break;
                    }
            }
        }

        #endregion

        private void OnSystemsInitialized()
        {
            contentLoader = systemsInitializer.GetSystem<ContentLoader>();
            uiManager = systemsInitializer.GetSystem<UiManager>();
            audioManager = systemsInitializer.GetSystem<AudioManager>();
            cameraStackingManager = systemsInitializer.GetSystem<CameraStackingManager>();
            localizationManager = systemsInitializer.GetSystem<LocalizationManager>();
            mainLevelSystems = new List<BaseSystem>();
            mainLevelSystemsInitializer = new SystemsInitializer();
            LoadPlayerData();
            localizationManager.UpdateSelectedLanguage(playerInformation.LanguageSelected);
            audioManager.UpdateMusicVolume(playerInformation.MusicVolumeSet);
            audioManager.UpdateVFXMusicVolume(playerInformation.VfxVolumeSet);
            CreateInputControllers();
            ShowMainMenuElements();
        }

        private void ShowMainMenuElements()
        {
            uiManager.DisplayView(ViewIds.AIRCRAFT_SHOWCASE, disableCurrentInteractableGroup: false);
            EventDispatcher.Instance.Dispatch(UiEvents.OnSetShowcaseAircraft, playerInformation.AircraftSelected);
            uiManager.DisplayView(ViewIds.MAIN_MENU, disableCurrentInteractableGroup: false);
            inputManager.EnableInput(uiManager);
        }

        private void CreateInputControllers()
        {

#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
            inputManager = new InputManager(enableTouchControls: true);
#else     
            inputManager = new InputManager(enableTouchControls: false);
#endif

            InputController[] inputControllers =
            {
                new UiController(this),
                new GameplayController(contentLoader)
            };

            inputManager.AddInputController(inputControllers);
        }

        private void HandleUiEvents(UiEvents uiEvent, object data)
        {
            switch (uiEvent)
            {
                case UiEvents.OnPlayButtonPressed:
                    {
                        inputManager.DisableInput(uiManager);
                        contentLoader.LoadScene("MainLevel", LoadSceneMode.Additive,
                            () =>
                            {
                                uiManager.RemoveView(ViewIds.AIRCRAFT_SHOWCASE);
                                uiManager.RemoveView(ViewIds.MAIN_MENU);
                                mainLevelSystems.Add(new LevelInitializer(playerInformation.AircraftSelected, uiManager, contentLoader, audioManager, cameraStackingManager));
                                mainLevelSystemsInitializer.OnSystemsInitialized += OnMainLevelSystemsInitialized;
                                mainLevelSystemsInitializer.InitializeSystems(mainLevelSystems);
                            });
                        break;
                    }

                case UiEvents.OnPauseButtonPressed:
                    {
                        uiManager.DisplayView(ViewIds.PAUSE_MENU, disableCurrentInteractableGroup: true);
                        inputManager.EnableInput(uiManager);
                        inputManager.DisableInput(currentLevelInitializer.Aircraft);
                        audioManager.PauseGameplayClips();
                        audioManager.PauseAllLoopingClips();
                        IsGamePaused = true;
                        break;
                    }

                case UiEvents.OnUnpauseButtonPressed:
                    {
                        uiManager.RemoveView(ViewIds.PAUSE_MENU);
                        inputManager.DisableInput(uiManager);
                        inputManager.EnableInput(currentLevelInitializer.Aircraft);
                        audioManager.UnPauseGameplayClips();
                        audioManager.UnPauseAllLoopingClips();
                        IsGamePaused = false;
                        break;
                    }

                case UiEvents.OnMainMenuButtonPressed:
                    {
                        IsGamePaused = false;
                        uiManager.RemoveView(ViewIds.PAUSE_MENU);
                        inputManager.DisableInput(currentLevelInitializer.Aircraft);
                        UnloadMainLevel();
                        break;
                    }

                case UiEvents.OnOptionsButtonPressed:
                    {
                        uiManager.DisplayView(ViewIds.OPTIONS_MENU, disableCurrentInteractableGroup: true);
                        break;
                    }

                case UiEvents.OnShopButtonPressed:
                    {
                        ShopViewData shopViewData = new ShopViewData(playerInformation);
                        uiManager.DisplayView(ViewIds.SHOP_MENU, disableCurrentInteractableGroup: true, shopViewData);
                        break;
                    }

                case UiEvents.OnQuitButtonPressed:
                    {
                        Application.Quit();
                        break;
                    }

                case UiEvents.OnMusicVolumeSliderUpdated:
                    {
                        float volume = (float)data;
                        playerInformation.MusicVolumeSet = volume;
                        storageAccessor.Save(playerInformation);
                        audioManager.UpdateMusicVolume(volume);
                        break;
                    }

                case UiEvents.OnVfxVolumeSliderUpdated:
                    {
                        float volume = (float)data;
                        playerInformation.VfxVolumeSet = volume;
                        storageAccessor.Save(playerInformation);
                        audioManager.UpdateVFXMusicVolume(volume);
                        break;
                    }

                case UiEvents.OnSelectAircraftButtonPressed:
                    {
                        string aircraftId = data as string;
                        playerInformation.AircraftSelected = aircraftId;
                        storageAccessor.Save(playerInformation);
                        break;
                    }

                case UiEvents.OnPurchaseAircraftButtonPressed:
                    {
                        Tuple<string, int> aircraftIdPricePair = data as Tuple<string, int>;
                        string aircraftId = aircraftIdPricePair.Item1;
                        int price = aircraftIdPricePair.Item2;

                        if (price > playerInformation.CoinsCollected)
                        {
                            string notEnoughCoinsMessage = localizationManager.GetLocalizedText("Message.NotEnoughCoins");
                            MessageViewData messageViewData = new MessageViewData(notEnoughCoinsMessage);
                            uiManager.DisplayView(ViewIds.MESSAGE_WINDOW, disableCurrentInteractableGroup: true, messageViewData);
                        }
                        else
                        {
                            playerInformation.CoinsCollected = playerInformation.CoinsCollected - price;
                            playerInformation.AircraftsPurchased.Add(aircraftId);
                            storageAccessor.Save(playerInformation);
                            EventDispatcher.Instance.Dispatch(UiEvents.OnUpdatePlayerInformation, playerInformation);
                        }

                        break;
                    }

                case UiEvents.OnLanguageButtonPressed:
                    {
                        SystemLanguage languageSelected = (SystemLanguage)data;
                        playerInformation.LanguageSelected = languageSelected;
                        storageAccessor.Save(playerInformation);
                        localizationManager.UpdateSelectedLanguage(languageSelected);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        private void OnMainLevelSystemsInitialized()
        {
            mainLevelSystemsInitializer.OnSystemsInitialized -= OnMainLevelSystemsInitialized;
            gameSessionCoinsCollected = 0;
            gameSessionWallsEvaded = 0;
            currentLevelInitializer = mainLevelSystemsInitializer.GetSystem<LevelInitializer>();
            audioManager.PlayBackgroundMusic(ClipIds.MUSIC_CLIP);
            inputManager.EnableInput(currentLevelInitializer.Aircraft);
            GameplayController gameplayContoller = inputManager.GetInputController(currentLevelInitializer.Aircraft) as GameplayController;

            if (gameplayContoller.VirtualJoystickEnabled)
            {
                uiManager.DisplayView(ViewIds.TOUCH_CONTROLS, disableCurrentInteractableGroup: false);
                TouchControlsView touchControlsView = uiManager.GetTopStackView(ViewIds.TOUCH_CONTROLS) as TouchControlsView;
                gameplayContoller.VirtualJoystick.OnTouchStart += touchControlsView.OnInitialPositionUpdated;
                gameplayContoller.VirtualJoystick.OnTouchDrag += touchControlsView.OnDragPositionUpdated;
                gameplayContoller.VirtualJoystick.OnTouchEnd += touchControlsView.OnEndPositionUpdated;
            }
        }

        private void HandleGameplayEvents(GameplayEvents gameplayEvent, object data)
        {
            switch (gameplayEvent)
            {
                case GameplayEvents.OnWallcollision:
                    {
                        storageAccessor.Save(playerInformation);
                        inputManager.DisableInput(currentLevelInitializer.Aircraft);
                        break;
                    }

                case GameplayEvents.OnWallEvaded:
                    {
                        playerInformation.WallsEvaded++;
                        gameSessionWallsEvaded++;
                        string wallsEvadedAsString = gameSessionWallsEvaded.ToString();
                        EventDispatcher.Instance.Dispatch(UiEvents.OnWallsValueChanged, wallsEvadedAsString);
                        break;
                    }

                case GameplayEvents.OnCoinCollected:
                    {
                        playerInformation.CoinsCollected++;
                        gameSessionCoinsCollected++;
                        string coinsCollectedAsString = gameSessionCoinsCollected.ToString();
                        EventDispatcher.Instance.Dispatch(UiEvents.OnCoinsValueChanged, coinsCollectedAsString);
                        break;
                    }

                case GameplayEvents.OnAircraftDestroyed:
                    {
                        UnloadMainLevel();
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        private void UnloadMainLevel()
        {
            GameplayController gameplayController = inputManager.GetInputController(currentLevelInitializer.Aircraft) as GameplayController;
            
            if (gameplayController.VirtualJoystickEnabled)
            {
                TouchControlsView touchControlsView = uiManager.GetTopStackView(ViewIds.TOUCH_CONTROLS) as TouchControlsView;
                gameplayController.VirtualJoystick.OnTouchStart -= touchControlsView.OnInitialPositionUpdated;
                gameplayController.VirtualJoystick.OnTouchDrag -= touchControlsView.OnDragPositionUpdated;
                gameplayController.VirtualJoystick.OnTouchEnd -= touchControlsView.OnEndPositionUpdated;
                uiManager.RemoveView(ViewIds.TOUCH_CONTROLS);
            }

            audioManager.StopAllLoopingClips();
            currentLevelInitializer.Dispose();
            currentLevelInitializer = null;
            mainLevelSystems.Clear();
            mainLevelSystemsInitializer.Dispose();
            contentLoader.UnloadScene("MainLevel", null);
            ShowMainMenuElements();
        }

        //NOTE: Make this function be part of a system so that other classes can access it.
        private void LoadPlayerData()
        {
            playerInformation = new PlayerInformation(
                sourceCoinsCollected: 0, 
                sourceWallsEvaded: 0, 
                sourceMusicVolumeSet: 1, 
                sourceVfxVolumeSet: 1,
                sourceAircraftSelected: AircraftIds.PLANE,
                sourceAircraftsPurchased: new List<string>() { AircraftIds.PLANE },
                sourceLanguageSelected: localizationManager.LanguageSelected);

            storageAccessor = new StorageAccessor();

            if (storageAccessor.DoesInformationExist(playerInformation.Key))
            {
                PlayerInformation playerInformationFound = storageAccessor.Load<PlayerInformation>(playerInformation.Key);
                playerInformationFound.TrasferValidValues(ref playerInformation);
            }
        }
    }

}

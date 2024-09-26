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

    public class GameManager : IListener
    {
        private SystemsInitializer systemsInitializer = null;
        private List<BaseSystem> baseSystems = null;
        private ContentLoader contentLoader = null;
        private UiManager uiManager = null;
        private AudioManager audioManager = null;
        private InputManager inputManager = null;
        private LevelInitializer currentLevelInitializer = null;
        private PlayerInformation playerInformation = null;
        private StorageAccessor storageAccessor = null;

        public bool IsGamePaused { get; private set; } = false;

        public GameManager()
        {
            playerInformation = new PlayerInformation(0, 0);
            storageAccessor = new StorageAccessor();
            baseSystems = new List<BaseSystem>();
            baseSystems.Add(new ContentLoader());
            baseSystems.Add(new UiManager()
                .AddDependency<ContentLoader>()
                .AddDependency<AudioManager>());
            baseSystems.Add(new AudioManager().AddDependency<ContentLoader>());
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
            }
        }

        #endregion

        private void OnSystemsInitialized()
        {
            contentLoader = systemsInitializer.GetSystem<ContentLoader>();
            uiManager = systemsInitializer.GetSystem<UiManager>();
            audioManager = systemsInitializer.GetSystem<AudioManager>();
            CreateInputControllers();
        }

        private void CreateInputControllers()
        {
            InputController[] inputControllers =
            {
                new UiController(this),
                new GameplayController()
            };

            inputManager = new InputManager(inputControllers);
            inputManager.EnableInput(uiManager);
        }

        private void HandleUiEvents(UiEvents uiEvent, object data)
        {
            switch (uiEvent)
            {
                case UiEvents.OnPlayButtonPressed:
                    {
                        contentLoader.LoadScene("MainLevel", LoadSceneMode.Additive,
                            () =>
                            {
                                uiManager.RemoveView(ViewIds.MAIN_MENU);
                                uiManager.DisplayView(ViewIds.HUD);
                                inputManager.DisableInput(uiManager);
                                currentLevelInitializer = new LevelInitializer(contentLoader, inputManager, audioManager);
                            });
                        break;
                    }

                case UiEvents.OnPauseButtonPressed:
                    {
                        uiManager.DisplayView(ViewIds.PAUSE_MENU);
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
                        UnloadMainLevel();
                        break;
                    }

                case UiEvents.OnQuitButtonPressed:
                    {
                        Debug.LogWarning(storageAccessor.Load<PlayerInformation>(playerInformation.Key).coinsCollected);
                        Application.Quit();
                        break;
                    }
            }
        }

        private void HandleGameplayEvents(GameplayEvents gameplayEvent, object data)
        {
            switch (gameplayEvent)
            {
                case GameplayEvents.OnWallcollision:
                    {
                        inputManager.DisableInput(currentLevelInitializer.Aircraft);
                        break;
                    }

                case GameplayEvents.OnWallEvaded:
                    {
                        playerInformation.wallsEvaded++;
                        string wallsEvadedAsString = playerInformation.wallsEvaded.ToString();
                        EventDispatcher.Instance.Dispatch(UiEvents.OnWallsValueChanged, wallsEvadedAsString);
                        break;
                    }

                case GameplayEvents.OnCoinCollected:
                    {
                        playerInformation.coinsCollected++;
                        string coinsCollectedAsString = playerInformation.coinsCollected.ToString();
                        EventDispatcher.Instance.Dispatch(UiEvents.OnCoinsValueChanged, coinsCollectedAsString);
                        break;
                    }

                case GameplayEvents.OnAircraftDestroyed:
                    {
                        storageAccessor.Save(playerInformation);
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
            uiManager.RemoveView(ViewIds.HUD);
            currentLevelInitializer.Dispose();
            currentLevelInitializer = null;
            contentLoader.UnloadScene("MainLevel",
            () =>
            {
                ResetPlayerData();
                uiManager.DisplayView(ViewIds.MAIN_MENU);
            });
        }

        private void ResetPlayerData()
        {
            playerInformation = new PlayerInformation(0, 0);
        }
    }

}

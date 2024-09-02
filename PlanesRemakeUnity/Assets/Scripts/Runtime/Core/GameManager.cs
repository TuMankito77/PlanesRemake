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

    public class GameManager : IListener
    {
        public struct PlayerData
        {
            public int coinsCollected;
            public int wallsEvaded;
        }

        private SystemsInitializer systemsInitializer = null;
        private List<BaseSystem> baseSystems = null;
        private ContentLoader contentLoader = null;
        private UiManager uiManager = null;
        private InputManager inputManager = null;
        private LevelInitializer currentLevelInitializer = null;
        private PlayerData playerData = default(PlayerData);

        public bool IsGamePaused { get; private set; } = false;

        public GameManager()
        {
            baseSystems = new List<BaseSystem>();
            baseSystems.Add(new ContentLoader());
            baseSystems.Add(new UiManager().AddDependency<ContentLoader>());
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
            playerData = new PlayerData();
            playerData.coinsCollected = 0;
            playerData.wallsEvaded = 0;
            CreateInputControllers();
        }

        private void CreateInputControllers()
        {
            InputController[] inputControllers =
            {
                new UiController(),
                new GameplayController(this)
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
                                uiManager.RemoveView(ViewIds.MainMenu);
                                uiManager.DisplayView(ViewIds.Hud);
                                inputManager.DisableInput(uiManager);
                                currentLevelInitializer = new LevelInitializer(contentLoader, inputManager);
                            });
                        break;
                    }

                case UiEvents.OnPauseButtonPressed:
                    {
                        uiManager.DisplayView(ViewIds.PauseMenu);
                        inputManager.EnableInput(uiManager);
                        IsGamePaused = true;
                        break;
                    }

                case UiEvents.OnUnpauseButtonPressed:
                    {
                        uiManager.RemoveView(ViewIds.PauseMenu);
                        inputManager.DisableInput(uiManager);
                        IsGamePaused = false;
                        break;
                    }

                case UiEvents.OnMainMenuButtonPressed:
                    {
                        uiManager.RemoveView(ViewIds.PauseMenu);
                        UnloadMainLevel();
                        break;
                    }

                case UiEvents.OnQuitButtonPressed:
                    {
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
                        playerData.wallsEvaded++;
                        string wallsEvadedAsString = playerData.wallsEvaded.ToString();
                        EventDispatcher.Instance.Dispatch(UiEvents.OnWallsValueChanged, wallsEvadedAsString);
                        break;
                    }

                case GameplayEvents.OnCoinCollected:
                    {
                        playerData.coinsCollected++;
                        string coinsCollectedAsString = playerData.coinsCollected.ToString();
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
            uiManager.RemoveView(ViewIds.Hud);
            currentLevelInitializer.Dispose();
            currentLevelInitializer = null;
            contentLoader.UnloadScene("MainLevel",
            () =>
            {
                ResetPlayerData();
                uiManager.DisplayView(ViewIds.MainMenu);
            });
        }

        private void ResetPlayerData()
        {
            playerData.coinsCollected = 0;
            playerData.wallsEvaded = 0;
        }
    }

}

namespace PlanesRemastered.Runtime.Core
{
    using System;
    using System.Collections.Generic;
    
    using UnityEngine;
    using UnityEngine.SceneManagement;
    
    using PlanesRemastered.Runtime.Events;
    using PlanesRemastered.Runtime.Gameplay;
    using PlanesRemastered.Runtime.Input;
    using PlanesRemastered.Runtime.UI;
    using PlanesRemastered.Runtime.UI.Views;

    public class GameManager : IListener
    {
        private SystemsInitializer systemsInitializer = null;
        private List<BaseSystem> baseSystems = null;
        private ContentLoader contentLoader = null;
        private UiManager uiManager = null;
        private InputManager inputManager = null;
        private LevelInitializer currentLevelInitializer = null;

        public bool IsGamePaused { get; private set; } = false;
        
        public GameManager()
        {
            baseSystems = new List<BaseSystem>();
            baseSystems.Add(new ContentLoader());
            baseSystems.Add(new UiManager().AddDependency<ContentLoader>());
            systemsInitializer = new SystemsInitializer();
            systemsInitializer.OnSystemsInitialized += OnSystemsInitialized;
            systemsInitializer.InitializeSystems(baseSystems);
            
            EventDispatcher.Instance.AddListener(this, typeof(UiEvents));
        }

        ~GameManager()
        {
            systemsInitializer.OnSystemsInitialized -= OnSystemsInitialized;
        }

        #region IListener

        public void HandleEvent(IComparable eventName, object data)
        {
            switch(eventName)
            {
                case UiEvents uiEvent:
                {
                    HandleUiEvents(uiEvent, data);
                    break;
                }
            }
        }

        #endregion

        private void OnSystemsInitialized()
        {
            contentLoader = systemsInitializer.GetSystem<ContentLoader>();
            uiManager = systemsInitializer.GetSystem<UiManager>();
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
            switch(uiEvent)
            {
                case UiEvents.OnPlayButtonPressed:
                    {
                        contentLoader.LoadScene("MainLevel", LoadSceneMode.Single, 
                            () =>
                            {
                                uiManager.RemoveView(ViewIds.MainMenu);
                                uiManager.DisplayView(ViewIds.Hud);
                                inputManager.DisableInput(uiManager);
                                currentLevelInitializer = new LevelInitializer(contentLoader, inputManager);
                            });
                        break;
                    }

                case UiEvents.OnPuauseButtonPressed:
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

                case UiEvents.OnQuitButtonPressed:
                    {
                        Application.Quit();
                        break;
                    }
            }
        }
    }

}

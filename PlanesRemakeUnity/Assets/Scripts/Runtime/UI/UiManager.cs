namespace PlanesRemake.Runtime.UI
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.EventSystems;

    using PlanesRemake.Runtime.Core;
    using PlanesRemake.Runtime.UI.Views;
    using PlanesRemake.Runtime.Input;
    using PlanesRemake.Runtime.Sound;

    public class UiManager : BaseSystem, IInputControlableEntity
    {
        private const string VIEWS_CONTAINER_SCRIPTABLE_OBJECT_PATH = "Ui/ViewsContainer";

        private ViewsContainer viewsContainer = null;
        private GameObject uiManagerGO = null;
        private Camera uiCamera = null;
        private List<BaseView> viewsOpened = null;
        private AudioManager audioManager = null;

        //To-do: Create a request class that will be sent through an event in order to request a view.
        public override async Task<bool> Initialize(IEnumerable<BaseSystem> sourceDependencies)
        {
            await base.Initialize(sourceDependencies);

            viewsOpened = new List<BaseView>();
            audioManager = GetDependency<AudioManager>();
            //To-do: Create a database that categorizes the objects loaded based on a list that groups what is needed to be loaded depending on what needs to be shown.
            bool isLoadingViewsContainer = true;
            ContentLoader contentLoader = GetDependency<ContentLoader>();
            contentLoader.LoadAsset<ViewsContainer>
                (VIEWS_CONTAINER_SCRIPTABLE_OBJECT_PATH,
                (assetLoaded) =>
                {
                    viewsContainer = assetLoaded;
                    isLoadingViewsContainer = false;
                },
                () => isLoadingViewsContainer = false);

            while(isLoadingViewsContainer)
            {
                await Task.Yield();
            }

            viewsContainer.Initialize();

            uiManagerGO = new GameObject("UI Manager");
            GameObject.DontDestroyOnLoad(uiManagerGO);

            uiCamera = new GameObject("UI Camera").AddComponent<Camera>();
            uiCamera.cullingMask = viewsContainer.ViewsLayerMask;
            uiCamera.clearFlags = CameraClearFlags.Depth;
            //Have a system that manages the order of derendering for the cameras
            uiCamera.depth = 1;
            uiCamera.gameObject.transform.SetParent(uiManagerGO.transform);

            GameObject eventSystem = new GameObject("Event System");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            eventSystem.transform.SetParent(uiManagerGO.transform);

            //To-do: Move the showing of the main menu view to the game manager.
            DisplayView(ViewIds.MainMenu);

            return true;
        }

        public void DisplayView(string viewId)
        {
            Debug.Assert(viewsContainer.ViewsById.ContainsKey(viewId), 
                $"{GetType().Name} - The view {viewId} id does not exist!");
            
            BaseView viewFound = GameObject.Instantiate(viewsContainer.ViewsById[viewId], uiManagerGO.transform);
            viewFound.Initialize(uiCamera, audioManager);
            viewFound.Canvas.sortingOrder = viewsOpened.Count;
            viewFound.TransitionIn();
            viewFound.transform.SetParent(uiManagerGO.transform);
            viewsOpened.Add(viewFound);
        }

        public void RemoveView(string viewId)
        {
            Debug.Assert(viewsContainer.ViewsById.ContainsKey(viewId), 
                $"{GetType().Name} - The view {viewId} id does not exist!");
            
            Type viewType = viewsContainer.ViewsById[viewId].GetType();
            
            if(!viewsOpened.Exists((view) => view.GetType() == viewType))
            {
                return;
            }

            BaseView viewFound = viewsOpened.FindLast((view) => view.GetType() == viewType);
            int viewIndex = viewFound.Canvas.sortingOrder;
            viewFound.TransitionOut();
            viewFound.Dispose();
            viewsOpened.Remove(viewFound);

            for(int i = viewIndex; i < viewsOpened.Count; i++)
            {
                viewsOpened[i].Canvas.sortingOrder = i;
            }

            //To-do: Make this be handled by a pool so that it can be reused.
            GameObject.Destroy(viewFound.gameObject);
        }
    }
}

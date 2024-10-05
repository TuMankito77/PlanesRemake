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
        private ViewsDatabase viewsDatabase = null;
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
            contentLoader.LoadAsset<ViewsDatabase>
                (ViewsDatabase.VIEWS_DATABASE_SCRIPTABLE_OBJECT_PATH,
                (assetLoaded) =>
                {
                    viewsDatabase = assetLoaded;
                    isLoadingViewsContainer = false;
                },
                () => isLoadingViewsContainer = false);

            while(isLoadingViewsContainer)
            {
                await Task.Yield();
            }

            viewsDatabase.Initialize();

            uiManagerGO = new GameObject("UI Manager");
            GameObject.DontDestroyOnLoad(uiManagerGO);

            uiCamera = new GameObject("UI Camera").AddComponent<Camera>();
            uiCamera.cullingMask = viewsDatabase.ViewsLayerMask;
            uiCamera.clearFlags = CameraClearFlags.Depth;
            //Have a system that manages the order of derendering for the cameras
            uiCamera.depth = 1;
            uiCamera.gameObject.transform.SetParent(uiManagerGO.transform);

            GameObject eventSystem = new GameObject("Event System");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            eventSystem.transform.SetParent(uiManagerGO.transform);

            return true;
        }

        public BaseView DisplayView(string viewId)
        {   
            BaseView viewFound = GameObject.Instantiate(viewsDatabase.GetFile(viewId), uiManagerGO.transform);
            viewFound.Initialize(uiCamera, audioManager);
            viewFound.Canvas.sortingOrder = viewsOpened.Count;
            viewFound.TransitionIn();
            viewFound.transform.SetParent(uiManagerGO.transform);
            viewsOpened.Add(viewFound);
            return viewFound;
        }

        public void RemoveView(string viewId)
        {
            Type viewType = viewsDatabase.GetFile(viewId).GetType();
            
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

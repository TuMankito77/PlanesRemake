namespace PlanesRemake.Runtime.UI
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using UnityEngine;

    using PlanesRemake.Runtime.Core;
    using PlanesRemake.Runtime.UI.Views;
    using PlanesRemake.Runtime.Input;
    using PlanesRemake.Runtime.Sound;
    using PlanesRemake.Runtime.Utils;
    using UnityEngine.InputSystem.UI;
    using UnityEngine.Rendering.Universal;

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
            ContentLoader contentLoader = GetDependency<ContentLoader>();
            viewsDatabase = await contentLoader.LoadAsset<ViewsDatabase>(ViewsDatabase.VIEWS_DATABASE_SCRIPTABLE_OBJECT_PATH);
            viewsDatabase.Initialize();

            uiManagerGO = new GameObject("UI Manager");
            GameObject.DontDestroyOnLoad(uiManagerGO);

            uiCamera = new GameObject("UI Camera").AddComponent<Camera>();
            uiCamera.cullingMask = viewsDatabase.ViewsLayerMask;
            uiCamera.clearFlags = CameraClearFlags.Nothing;
            uiCamera.gameObject.transform.SetParent(uiManagerGO.transform);
            uiCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            CameraStackingManager cameraStackingManager = GetDependency<CameraStackingManager>();
            cameraStackingManager.AddCameraToStackAtTop(uiCamera);

            GameObject eventSystem = new GameObject("Event System");
            eventSystem.AddComponent<InputSystemUIInputModule>();
            eventSystem.transform.SetParent(uiManagerGO.transform);

            return true;
        }

        public BaseView DisplayView(string viewId)
        {
            if(viewsOpened.Count > 0)
            {
                CurrentViewDisplayed().SetInteractable(false);
            }

            BaseView viewFound = GameObject.Instantiate(viewsDatabase.GetFile(viewId), uiManagerGO.transform);
            viewFound.Initialize(uiCamera, audioManager);
            //NOTE: This will update the values like the width and height so that they do not appear as zero,
            //dunno how I will remind this to myself -_-, BUT remember, we have to do this before trying to access any RectTransform values
            Canvas.ForceUpdateCanvases();
            viewFound.Canvas.sortingOrder = viewsOpened.Count;
            viewFound.TransitionIn();
            viewFound.transform.SetParent(uiManagerGO.transform);
            viewsOpened.Add(viewFound);
            return viewFound;
        }

        public BaseView GetTopStackView(string viewId)
        {
            Type viewType = viewsDatabase.GetFile(viewId).GetType();

            for (int i = viewsOpened.Count - 1; i >= 0; i--)
            {
                if (viewsOpened[i].GetType() == viewType)
                {
                    return viewsOpened[i];
                }
            }

            LoggerUtil.LogError($"{GetType()}: There is no view with the id {viewId} being displayed currently.");
            return null;
        }

        public BaseView CurrentViewDisplayed()
        {
            return viewsOpened[viewsOpened.Count - 1];
        }

        public void RemoveView(string viewId)
        {
            Type viewType = viewsDatabase.GetFile(viewId).GetType();
            
            if(!viewsOpened.Exists((view) => view.GetType() == viewType))
            {
                return;
            }

            BaseView viewFound = viewsOpened.FindLast((view) => view.GetType() == viewType);
            RemoveView(viewFound);
        }

        public void RemoveTopStackView()
        {
            //We make sure that there will always be at least one view opened.
            if(viewsOpened.Count <= 1)
            {
                return;
            }

            int lastIndex = viewsOpened.Count - 1;
            BaseView topStackView = viewsOpened[lastIndex];
            RemoveView(topStackView);
        }

        private void RemoveView(BaseView view)
        {
            int viewIndex = view.Canvas.sortingOrder;
            viewsOpened.Remove(view);

            void OnTransitionOutFinished()
            {
                view.onTransitionOutFinished -= OnTransitionOutFinished;
                view.Dispose();

                for (int i = viewIndex; i < viewsOpened.Count; i++)
                {
                    viewsOpened[i].Canvas.sortingOrder = i;
                }

                //To-do: Make this be handled by a pool so that it can be reused.
                GameObject.Destroy(view.gameObject);
                CurrentViewDisplayed().SetInteractable(true);
            }

            view.onTransitionOutFinished += OnTransitionOutFinished;
            view.TransitionOut();
        }
    }
}

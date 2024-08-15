namespace PlanesRemastered.Runtime.UI.Views 
{
    using UnityEngine;

    [RequireComponent(typeof (Canvas),typeof(CanvasGroup))]
    public abstract class BaseView : MonoBehaviour
    {
        public Canvas Canvas { get; private set; } = null;
        public CanvasGroup CanvasGroup { get; private set; } = null;

        public virtual void Initialize(Camera uiCamera)
        {
            Canvas = GetComponent<Canvas>();
            CanvasGroup = GetComponent<CanvasGroup>();
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            Canvas.worldCamera = uiCamera;
        }

        public virtual void Dispose()
        {
            Canvas = null;
            CanvasGroup = null;
        }

        public virtual void TransitionIn()
        {
            CanvasGroup.alpha = 1;
            CanvasGroup.interactable = true;
        }

        public virtual void TransitionOut()
        {
            CanvasGroup.alpha = 0;
            CanvasGroup.interactable = false;
        }
    }
}


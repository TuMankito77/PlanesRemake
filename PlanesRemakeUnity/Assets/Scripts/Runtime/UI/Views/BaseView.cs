namespace PlanesRemake.Runtime.UI.Views 
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Sound;
    using PlanesRemake.Runtime.UI.CoreElements;
    using UnityEngine.UI;
    using System;

    [RequireComponent(typeof (Canvas),typeof(CanvasGroup))]
    public abstract class BaseView : MonoBehaviour
    {
        public event Action onTransitionInFinished;
        public event Action onTransitionOutFinished;

        public Canvas Canvas { get; private set; } = null;
        public CanvasGroup CanvasGroup { get; private set; } = null;
        public CanvasScaler CanvasScaler { get; private set; } = null;

        protected AudioManager audioManager = null;
        private BaseButton[] buttons = new BaseButton[0];
        private IViewAnimator viewAnimator = null;

        #region Unity Methods

        protected virtual void Awake()
        {
            buttons = GetComponentsInChildren<BaseButton>();

            foreach(BaseButton button in buttons)
            {
                button.onSubmit += OnButtonSubmit;
            }

            viewAnimator = GetComponent<IViewAnimator>();

            if(viewAnimator != null)
            {
                viewAnimator.OnTransitionInAnimationCompleted += OnTransitionInAnimationCompleted;
                viewAnimator.OnTransitionOutAnimatonCompleted += OnTransitionOutAnimatonCompleted;
            }
        }


        protected virtual void OnDestroy()
        {
            foreach(BaseButton button in buttons)
            {
                button.onSubmit -= OnButtonSubmit;
            }

            if (viewAnimator != null)
            {
                viewAnimator.OnTransitionInAnimationCompleted -= OnTransitionInAnimationCompleted;
                viewAnimator.OnTransitionOutAnimatonCompleted -= OnTransitionOutAnimatonCompleted;
            }
        }

        #endregion

        public virtual void Initialize(Camera uiCamera, AudioManager sourceAudioManager)
        {
            Canvas = GetComponent<Canvas>();
            CanvasGroup = GetComponent<CanvasGroup>();
            CanvasScaler = GetComponent<CanvasScaler>();
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            Canvas.worldCamera = uiCamera;
            CanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            audioManager = sourceAudioManager;
        }

        public virtual void Dispose()
        {
            Canvas = null;
            CanvasGroup = null;
        }

        public virtual void TransitionIn()
        {
            CanvasGroup.alpha = 1;

            if(viewAnimator != null)
            {
                viewAnimator.PlayTransitionIn();
            }
            else
            {
                CanvasGroup.interactable = true;
                onTransitionInFinished?.Invoke();
            }
        }

        public virtual void TransitionOut()
        {
            CanvasGroup.interactable = false;

            if(viewAnimator != null)
            {
                viewAnimator.PlayTransitionOut();
            }
            else
            {
                CanvasGroup.alpha = 0;
                onTransitionOutFinished?.Invoke();
            }
        }

        private void OnButtonSubmit()
        {
            audioManager.PlayGeneralClip(ClipIds.BUTTON_CLICK_CLIP);
        }

        private void OnTransitionOutAnimatonCompleted()
        {
            CanvasGroup.alpha = 0;
            onTransitionOutFinished?.Invoke();
        }

        private void OnTransitionInAnimationCompleted()
        {
            CanvasGroup.interactable = true;
            onTransitionInFinished?.Invoke();
        }
    }
}


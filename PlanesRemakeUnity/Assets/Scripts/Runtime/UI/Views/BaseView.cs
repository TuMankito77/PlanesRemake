namespace PlanesRemake.Runtime.UI.Views 
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Sound;
    using PlanesRemake.Runtime.UI.CoreElements;
    using UnityEngine.UI;
    using System;
    using PlanesRemake.Runtime.UI.Views.DataContainers;

    [RequireComponent(typeof (Canvas),typeof(CanvasGroup))]
    public abstract class BaseView : MonoBehaviour
    {
        [SerializeField]
        private Image inputBlockerImage = null;

        public event Action onTransitionInFinished;
        public event Action onTransitionOutFinished;

        protected AudioManager audioManager = null;
        private BaseButton[] buttons = new BaseButton[0];
        private SelectableElement[] selectableElements = new SelectableElement[0];
        private IViewAnimator viewAnimator = null;
        private int interactableGroupId = -1;

        public Canvas Canvas { get; private set; } = null;
        public CanvasGroup CanvasGroup { get; private set; } = null;
        public CanvasScaler CanvasScaler { get; private set; } = null;
        public int InteractableGroupId { get => interactableGroupId; private set => interactableGroupId = value; }

        #region Unity Methods

        protected virtual void Awake()
        {
            buttons = GetComponentsInChildren<BaseButton>();
            selectableElements = GetComponentsInChildren<SelectableElement>();

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

        public void SetInteractable(bool isInractable)
        {
            inputBlockerImage.raycastTarget = !isInractable;
            CanvasGroup.interactable = isInractable;
        }

        public virtual void Initialize(Camera uiCamera, AudioManager sourceAudioManager, ViewInjectableData viewInjectableData)
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

        public virtual void TransitionIn(int sourceInteractableGroupId)
        {
            InteractableGroupId = sourceInteractableGroupId;
            CanvasGroup.alpha = 1;

            if(viewAnimator != null)
            {
                SetInteractable(false);
                viewAnimator.PlayTransitionIn();
            }
            else
            {
                SetInteractable(true);
                onTransitionInFinished?.Invoke();
            }
        }

        public virtual void TransitionOut()
        {
            SetInteractable(false);

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
            SetInteractable(true);
            onTransitionInFinished?.Invoke();
        }
    }
}


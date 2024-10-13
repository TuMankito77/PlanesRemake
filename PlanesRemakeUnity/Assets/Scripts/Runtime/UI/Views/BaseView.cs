namespace PlanesRemake.Runtime.UI.Views 
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Sound;
    using PlanesRemake.Runtime.UI.CoreElements;

    [RequireComponent(typeof (Canvas),typeof(CanvasGroup))]
    public abstract class BaseView : MonoBehaviour
    {
        public Canvas Canvas { get; private set; } = null;
        public CanvasGroup CanvasGroup { get; private set; } = null;

        protected AudioManager audioManager = null;
        private BaseButton[] buttons = new BaseButton[0];

        #region Unity Methods

        protected virtual void Awake()
        {
            buttons = GetComponentsInChildren<BaseButton>();

            foreach(BaseButton button in buttons)
            {
                button.onButtonPressed += OnButtonPressed;
            }
        }

        protected virtual void OnDestroy()
        {
            foreach(BaseButton button in buttons)
            {
                button.onButtonPressed -= OnButtonPressed;
            }
        }

        #endregion

        public virtual void Initialize(Camera uiCamera, AudioManager sourceAudioManager)
        {
            Canvas = GetComponent<Canvas>();
            CanvasGroup = GetComponent<CanvasGroup>();
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            Canvas.worldCamera = uiCamera;
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
            CanvasGroup.interactable = true;
        }

        public virtual void TransitionOut()
        {
            CanvasGroup.alpha = 0;
            CanvasGroup.interactable = false;
        }

        private void OnButtonPressed()
        {
            audioManager.PlayGeneralClip(ClipIds.BUTTON_CLICK_CLIP);
        }
    }
}


namespace PlanesRemake.Runtime.UI.Views
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.UI.CoreElements;

    public class MainMenuView : BaseView
    {
        [SerializeField]
        private BaseButton playButton = null;

        [SerializeField]
        private BaseButton optionsButton = null;
        
        [SerializeField]
        private BaseButton quitButton = null;

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            playButton.onButtonPressed += OnPlayButtonPressed;
            optionsButton.onButtonPressed += OnOptionsButtonPressed;
            quitButton.onButtonPressed += OnQuitButtonPressed;
        }

        protected override void OnDestroy()
        {
            base.Awake();
            playButton.onButtonPressed -= OnPlayButtonPressed;
            optionsButton.onButtonPressed -= OnOptionsButtonPressed;
            quitButton.onButtonPressed -= OnQuitButtonPressed;
        }

        #endregion

        private void OnPlayButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnPlayButtonPressed);
        }

        private void OnOptionsButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnOptionsButtonPressed);
        }
        
        private void OnQuitButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnQuitButtonPressed);
        }
    }
}

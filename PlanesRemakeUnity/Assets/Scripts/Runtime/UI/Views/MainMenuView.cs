namespace PlanesRemake.Runtime.UI.Views
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Events;

    public class MainMenuView : BaseView
    {
        [SerializeField]
        private BaseButton playButton = null;

        [SerializeField]
        private BaseButton quitButton = null;

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            playButton.onButtonPressed += OnPlayButtonPressed;
            quitButton.onButtonPressed += OnQuitButtonPressed;
        }

        protected override void OnDestroy()
        {
            base.Awake();
            playButton.onButtonPressed -= OnPlayButtonPressed;
            quitButton.onButtonPressed -= OnQuitButtonPressed;
        }

        #endregion

        private void OnPlayButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnPlayButtonPressed);
        }
        
        private void OnQuitButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnQuitButtonPressed);
        }
    }
}

namespace PlanesRemastered.Runtime.UI.Views
{
    using UnityEngine;
    
    using PlanesRemastered.Runtime.Events;

    public class MainMenuView : BaseView
    {
        [SerializeField]
        private BaseButton playButton = null;

        [SerializeField]
        private BaseButton quitButton = null;

        #region Unity Methods

        private void Awake()
        {
            playButton.onButtonPressed += OnPlayButtonPressed;
            quitButton.onButtonPressed += OnQuitButtonPressed;
        }

        private void OnDestroy()
        {
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

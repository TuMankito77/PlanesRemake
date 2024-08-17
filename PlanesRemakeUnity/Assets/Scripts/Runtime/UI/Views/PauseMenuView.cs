namespace PlanesRemastered.Runtime.UI.Views
{
    using UnityEngine;

    using PlanesRemastered.Runtime.Events;

    public class PauseMenuView : BaseView
    {
        [SerializeField]
        private BaseButton continueButton = null;

        [SerializeField]
        private BaseButton mainMenuButton = null;

        #region Unity Methods

        private void Awake()
        {
            continueButton.onButtonPressed += OnContinueButtonPressed;
            mainMenuButton.onButtonPressed += OnMainMenuButtonPressed;
        }

        private void OnDestroy()
        {
            continueButton.onButtonPressed -= OnContinueButtonPressed;
            mainMenuButton.onButtonPressed -= OnMainMenuButtonPressed;
        }

        #endregion

        private void OnContinueButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnUnpauseButtonPressed);
        }

        private void OnMainMenuButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnMainMenuButtonPressed);
        }
    }
}


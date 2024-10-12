namespace PlanesRemake.Runtime.UI.Views
{
    using UnityEngine;

    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.UI.CoreElements;

    public class PauseMenuView : BaseView
    {
        [SerializeField]
        private BaseButton continueButton = null;

        [SerializeField]
        private BaseButton mainMenuButton = null;

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            continueButton.onButtonPressed += OnContinueButtonPressed;
            mainMenuButton.onButtonPressed += OnMainMenuButtonPressed;
        }

        protected override void OnDestroy()
        {
            base.Awake();
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


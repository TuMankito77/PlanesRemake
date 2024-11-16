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
        private BaseButton optionsButton = null;

        [SerializeField]
        private BaseButton mainMenuButton = null;

        #region Unity Methods

        public override void TransitionIn(int sourceInteractableGroupId)
        {
            base.TransitionIn(sourceInteractableGroupId);
            continueButton.onButtonPressed += OnContinueButtonPressed;
            optionsButton.onButtonPressed += OnOptionsButtonPressed;
            mainMenuButton.onButtonPressed += OnMainMenuButtonPressed;
        }

        public override void TransitionOut()
        {
            base.TransitionOut();
            continueButton.onButtonPressed -= OnContinueButtonPressed;
            optionsButton.onButtonPressed -= OnOptionsButtonPressed;
            mainMenuButton.onButtonPressed -= OnMainMenuButtonPressed;
        }

        #endregion

        private void OnContinueButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnUnpauseButtonPressed);
        }

        private void OnOptionsButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnOptionsButtonPressed);
        }

        private void OnMainMenuButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnMainMenuButtonPressed);
        }
    }
}


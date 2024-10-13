namespace PlanesRemake.Runtime.UI
{
    using System;
    using PlanesRemake.Runtime.Core;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Input;
    using PlanesRemake.Runtime.UI.Views;

    public class UiController : InputController
    {
        public override Type EntityToControlType => typeof(UiManager);

        private UiManager uiManager = null;
        private GameManager gameManager = null;

        public UiController(GameManager sourceGameManager)
        {
            gameManager = sourceGameManager;
        }

        public override void Enable(InputActions sourceInputActions, IInputControlableEntity sourceEntityToControl)
        {
            base.Enable(sourceInputActions, entityToControl);
            inputActions.UiController.Enable();
            inputActions.UiController.GoBack.performed += GoBackActionTriggered;
            uiManager = sourceEntityToControl as UiManager;
        }

        private void GoBackActionTriggered(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if(uiManager.CurrentViewDisplayed().GetType() == typeof(PauseMenuView))
            {
                EventDispatcher.Instance.Dispatch(UiEvents.OnUnpauseButtonPressed);
                return;
            }

            uiManager.RemoveTopStackView();
        }

        public override void Disable()
        {
            inputActions.UiController.GoBack.performed -= GoBackActionTriggered;
            inputActions.UiController.Disable();
        }
    }
}
namespace PlanesRemake.Runtime.UI
{
    using System;
    using PlanesRemake.Runtime.Core;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Input;
    
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
            inputActions.UiController.Unpause.performed += OnUpauseActionTriggered;
            inputActions.UiController.GoBack.performed += GoBackActionTriggered;
            uiManager = sourceEntityToControl as UiManager;
        }

        private void GoBackActionTriggered(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            uiManager.RemoveTopStackView();
        }

        private void OnUpauseActionTriggered(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if(gameManager.IsGamePaused)
            {
                EventDispatcher.Instance.Dispatch(UiEvents.OnUnpauseButtonPressed);
            }
        }

        public override void Disable()
        {
            inputActions.UiController.Unpause.performed -= OnUpauseActionTriggered;
            inputActions.UiController.Disable();
        }
    }
}
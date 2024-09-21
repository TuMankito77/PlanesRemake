namespace PlanesRemake.Runtime.Gameplay
{
    using System;

    using UnityEngine;
    using UnityEngine.InputSystem;

    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Input;
    using PlanesRemake.Runtime.Core;

    public class GameplayController : InputController
    {
        public override Type EntityToControlType => typeof(Aircraft);

        private Aircraft aircraft = null;

        public override void Update()
        {
            Vector2 movementDirection = inputActions.GameplayController.Movement.ReadValue<Vector2>();
            aircraft.UpdateDirection(movementDirection);
        }

        public override void Enable(InputActions sourceInputActions, IInputControlableEntity sourceEntityToControl)
        {
            base.Enable(sourceInputActions, sourceEntityToControl);
            sourceInputActions.GameplayController.Enable();
            aircraft = sourceEntityToControl as Aircraft;
            sourceInputActions.GameplayController.Pause.performed += OnPuaseActionTriggered;
        }

        private void OnPuaseActionTriggered(InputAction.CallbackContext obj)
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnPauseButtonPressed);
        }

        public override void Disable()
        {
            inputActions.GameplayController.Pause.performed -= OnPuaseActionTriggered;
            inputActions.GameplayController.Disable();
        }
    }
}
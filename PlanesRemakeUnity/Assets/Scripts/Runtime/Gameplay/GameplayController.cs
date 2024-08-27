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
        private GameManager gameManager = null;

        public GameplayController(GameManager sourceGameManager)
        {
            gameManager = sourceGameManager;
        }

        public override void Update()
        {
            if(gameManager.IsGamePaused)
            {
                return;
            }

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
            IComparable eventToDispatch = gameManager.IsGamePaused ? UiEvents.OnUnpauseButtonPressed : UiEvents.OnPuauseButtonPressed;
            EventDispatcher.Instance.Dispatch(eventToDispatch);
        }

        public override void Disable()
        {
            inputActions.GameplayController.Disable();
        }
    }
}
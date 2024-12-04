namespace PlanesRemake.Runtime.UI
{
    using System;

    using UnityEngine;
    using UnityEngine.InputSystem;
    
    using PlanesRemake.Runtime.Core;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Input;
    using PlanesRemake.Runtime.UI.Views;
    using PlanesRemake.Runtime.UI.CoreElements;


    public class UiController : InputController
    {
        public override Type EntityToControlType => typeof(UiManager);

        private UiManager uiManager = null;
        private GameManager gameManager = null;

        public UiController(GameManager sourceGameManager)
        {
            gameManager = sourceGameManager;
        }

        public override void Update()
        {
            base.Update();

            if(inputActions.UiController.Navigate.ReadValue<Vector2>().magnitude == 0)
            {
                inputActions.UiController.Navigate.performed += OnNavigateActionTriggered;
            }
        }

        public override void Enable(InputActions sourceInputActions, IInputControlableEntity sourceEntityToControl)
        {
            base.Enable(sourceInputActions, entityToControl);
            inputActions.UiController.Enable();
            inputActions.UiController.GoBack.performed += GoBackActionTriggered;
            inputActions.UiController.Navigate.performed += OnNavigateActionTriggered;
            uiManager = sourceEntityToControl as UiManager;
        }

        //NOTE: We are handling navigation since letting unity's event system do it cuases a bug where
        //it start to choose random buttons at some point.
        private void OnNavigateActionTriggered(InputAction.CallbackContext context)
        {
            Vector2 direction = inputActions.UiController.Navigate.ReadValue<Vector2>();

            float deadZone = 0.5f;

            if(direction.magnitude < deadZone)
            {
                return;
            }

            if(direction.y > deadZone)
            {
                uiManager.CurrentViewDisplayed().SelectNeighborButton(SelectableNeighborDirection.Up);
            }

            if(direction.y < -deadZone)
            {
                uiManager.CurrentViewDisplayed().SelectNeighborButton(SelectableNeighborDirection.Down);
            }
            
            if(direction.x  > deadZone)
            {
                uiManager.CurrentViewDisplayed().SelectNeighborButton(SelectableNeighborDirection.Right);
            }

            if (direction.x < -deadZone)
            {
                uiManager.CurrentViewDisplayed().SelectNeighborButton(SelectableNeighborDirection.Left);
            }

            //NOTE: We unsubscribe and subscribe in the update in order to avoid registering the gamepad's stick position for every
            //small movment made which causes the selected button to change way too frequently.
            inputActions.UiController.Navigate.performed -= OnNavigateActionTriggered;
        }

        private void GoBackActionTriggered(InputAction.CallbackContext context)
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
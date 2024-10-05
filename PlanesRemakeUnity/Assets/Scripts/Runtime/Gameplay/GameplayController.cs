namespace PlanesRemake.Runtime.Gameplay
{
    using System;

    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.EnhancedTouch;

    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Input;
    using PlanesRemake.Runtime.Core;
    using VirtualJoystick = Input.TouchControls.Joystick;

    public class GameplayController : InputController
    {
        private const string VIRTUAL_JOYSTICK_PREFAB_PATH = "TouchControls/JoystickInput";

        public override Type EntityToControlType => typeof(Aircraft);

        private Aircraft aircraft = null;
        private VirtualJoystick virtualJoystick = null;

        public VirtualJoystick VirtualJoystick => virtualJoystick;
        public bool VirtualJoystickEnabled => virtualJoystick != null;

        public GameplayController(ContentLoader contentLoader)
        {
            if(EnhancedTouchSupport.enabled)
            {
                contentLoader.LoadAsset<VirtualJoystick>
                    (VIRTUAL_JOYSTICK_PREFAB_PATH,
                    (assetLoaded) =>
                    {
                        virtualJoystick = GameObject.Instantiate(assetLoaded, Vector3.zero, Quaternion.identity);
                        GameObject.DontDestroyOnLoad(virtualJoystick);
                    },
                    null);
            }
        }

        public override void Update()
        {
            Vector2 movementDirection = inputActions.GameplayController.Movement.ReadValue<Vector2>();
            aircraft.UpdateDirection(movementDirection);
        }

        public override void Enable(InputActions sourceInputActions, IInputControlableEntity sourceEntityToControl)
        {
            base.Enable(sourceInputActions, sourceEntityToControl);
            inputActions.GameplayController.Enable();
            aircraft = sourceEntityToControl as Aircraft;
            sourceInputActions.GameplayController.Pause.performed += OnPuaseActionTriggered;

            if(virtualJoystick != null)
            {
                virtualJoystick.RegisterToTouchEvents();
            }
        }

        public override void Disable()
        {
            inputActions.GameplayController.Pause.performed -= OnPuaseActionTriggered;
            inputActions.GameplayController.Disable();

            if(virtualJoystick != null)
            {
                virtualJoystick.DeregisterFromTouchEvents();
            }
        }

        private void OnPuaseActionTriggered(InputAction.CallbackContext obj)
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnPauseButtonPressed);
        }
    }
}
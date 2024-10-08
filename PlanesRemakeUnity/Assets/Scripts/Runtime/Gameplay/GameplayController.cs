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
        private VirtualJoystick virtualJoystickPrefab = null;
        private ContentLoader contentLoader = null;

        public VirtualJoystick VirtualJoystick => virtualJoystick;
        public bool VirtualJoystickEnabled => virtualJoystick != null;

        public GameplayController(ContentLoader sourceContentLoader)
        {
            contentLoader = sourceContentLoader;

            if(EnhancedTouchSupport.enabled)
            {
                virtualJoystickPrefab = contentLoader.LoadAssetSynchronously<VirtualJoystick>(VIRTUAL_JOYSTICK_PREFAB_PATH);
                virtualJoystick = GameObject.Instantiate(virtualJoystickPrefab, Vector3.zero, Quaternion.identity);
                GameObject.DontDestroyOnLoad(virtualJoystick);
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

        public override void Dispose()
        {
            base.Dispose();
            virtualJoystick = null;
            GameObject.Destroy(virtualJoystick.gameObject);
            contentLoader.UnloadAsset(virtualJoystickPrefab);
        }

        private void OnPuaseActionTriggered(InputAction.CallbackContext obj)
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnPauseButtonPressed);
        }
    }
}
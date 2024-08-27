namespace PlanesRemake.Runtime.UI
{
    using System;

    using PlanesRemake.Runtime.Input;
    
    public class UiController : InputController
    {
        public override Type EntityToControlType => typeof(UiManager);

        private UiManager uiManager = null;

        public override void Update()
        {

        }

        public override void Enable(InputActions sourceInputActions, IInputControlableEntity sourceEntityToControl)
        {
            base.Enable(sourceInputActions, entityToControl);
            inputActions.UiController.Enable();
            uiManager = sourceEntityToControl as UiManager;
        }

        public override void Disable()
        {
            inputActions.UiController.Disable();
        }
    }
}
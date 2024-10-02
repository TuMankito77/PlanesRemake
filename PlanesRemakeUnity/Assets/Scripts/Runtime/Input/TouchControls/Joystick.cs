namespace PlanesRemake.Runtime.Input.TouchControls
{
    using UnityEngine;
    using UnityEngine.InputSystem.EnhancedTouch;
    using UnityEngine.InputSystem.Layouts;
    using UnityEngine.InputSystem.OnScreen;
    using ETouch = UnityEngine.InputSystem.EnhancedTouch;

    public class Joystick : OnScreenControl
    {
        [InputControl(layout = "Button")]
        [SerializeField]
        private string controlPathSelected;

        [SerializeField, Min(0)]
        private float deadzoneRadius = 0;
        
        private Finger fingerMovement = null;
        private Vector2 initialPosition = Vector2.zero;
        private Vector2 movementAmount = Vector2.zero;

        protected override string controlPathInternal 
        { 
            get => controlPathSelected;
            set => controlPathSelected = value;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            RegisterToTouchEvents();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            DeregisterFromTouchEvents();
        }

        private void RegisterToTouchEvents()
        {
            //NOTE: Maybe this should be enabled somewhere else since we will not only have this class that 
            //enables the enhanced touch support.
            EnhancedTouchSupport.Enable();
            ETouch.Touch.onFingerDown += OnFingerDown;
            ETouch.Touch.onFingerMove += OnFingerMove;
            ETouch.Touch.onFingerUp += OnFingerUp;
        }

        private void DeregisterFromTouchEvents()
        {
            ETouch.Touch.onFingerDown -= OnFingerDown;
            ETouch.Touch.onFingerMove -= OnFingerMove;
            ETouch.Touch.onFingerUp -= OnFingerUp;
            //NOTE: Same as with enable, maybe this should be disabled somewhere else.
            //REMINDER: This should be called after all the classes have unsubscribed from the touch events.
            EnhancedTouchSupport.Disable();
        }

        private void OnFingerDown(Finger finger)
        {
            if(fingerMovement != null)
            {
                return;
            }
            
            fingerMovement = finger;
            initialPosition = finger.screenPosition;
        }

        private void OnFingerMove(Finger finger)
        {
            if(fingerMovement != finger)
            {
                return;
            }

            movementAmount = finger.screenPosition - initialPosition;
            
            if(movementAmount.magnitude > deadzoneRadius)
            {
                SendValueToControl(movementAmount.normalized);
            }
        }

        private void OnFingerUp(Finger finger)
        {
            if(fingerMovement != finger)
            {
                return;
            }

            fingerMovement = null;
            movementAmount = Vector2.zero;
            SendValueToControl(movementAmount);
        }
    }
}


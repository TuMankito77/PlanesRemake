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

        public float DeadzoneRadius { get => deadzoneRadius; set => deadzoneRadius = value; }

        protected override string controlPathInternal 
        { 
            get => controlPathSelected;
            set => controlPathSelected = value;
        }

        #region Unity Methods

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

        #endregion

        public void RegisterToTouchEvents()
        {
            ETouch.Touch.onFingerDown += OnFingerDown;
            ETouch.Touch.onFingerMove += OnFingerMove;
            ETouch.Touch.onFingerUp += OnFingerUp;
            //Making sure the movement is cleaned up when we are start
            movementAmount = Vector2.zero;
            SendValueToControl(movementAmount);
        }

        public void DeregisterFromTouchEvents()
        {
            ETouch.Touch.onFingerDown -= OnFingerDown;
            ETouch.Touch.onFingerMove -= OnFingerMove;
            ETouch.Touch.onFingerUp -= OnFingerUp;
            //Making sure the movement is cleaned up when we are done
            movementAmount = Vector2.zero;
            SendValueToControl(movementAmount);
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
            else
            {
                SendValueToControl(Vector2.zero);
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


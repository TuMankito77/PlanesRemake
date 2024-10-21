namespace PlanesRemake.Runtime.UI.Views
{
    using UnityEngine;

    public class TouchControlsView : BaseView
    {
        [SerializeField]
        private RectTransform joystickOuterCircle = null;

        [SerializeField]
        private RectTransform joystickInnerCircle = null;

        [SerializeField, Range(0.01f, 1)]
        private float screenResolutionScaleFactor = 0.1f;
        
        [SerializeField, Range(0, 1)]
        private float horizontalDefaultPlace = 0.25f;

        [SerializeField, Range(0, 1)]
        private float verticalDefaultPlace = 0.25f;
        
        private Vector2 initialPosition = Vector2.zero;
        private Vector2 defaultJoystickPosition = Vector2.zero;
        private Vector2 screenResolution = new Vector2(Screen.width, Screen.height);

        #region Unity Methods

        protected void Start()
        {
            float innerJoystickScaleFactor = joystickInnerCircle.sizeDelta.magnitude / joystickOuterCircle.sizeDelta.magnitude;
            screenResolution = new Vector2(Screen.width, Screen.height);
            float joystickSize = screenResolution.magnitude * screenResolutionScaleFactor;
            float innerJoystickSize = joystickSize * innerJoystickScaleFactor;
            joystickOuterCircle.sizeDelta = new Vector2(joystickSize, joystickSize);
            joystickInnerCircle.sizeDelta = new Vector2(innerJoystickSize, innerJoystickSize);
            defaultJoystickPosition = GetLowerLeftCornerAnchoredPosition() +
                new Vector2(screenResolution.x * horizontalDefaultPlace, screenResolution.y * verticalDefaultPlace);
            joystickOuterCircle.anchoredPosition = defaultJoystickPosition;
        }

        #endregion

        public void OnInitialPositionUpdated(Vector2 position)
        {
            Vector2 middleAchoredPosition = GetLowerLeftCornerAnchoredPosition() + position;
            joystickOuterCircle.anchoredPosition = middleAchoredPosition;
            joystickInnerCircle.anchoredPosition = Vector2.zero;
            initialPosition = position;
        }

        public void OnDragPositionUpdated(Vector2 position)
        {
            //NOTE: Since the inner circle is a child of the outer circle, to new position needs to be local, 
            //hense the subtraction.
            Vector2 newPosition = position - initialPosition;
            //NOTE: Clamping the magnitude to always remain withing the outer circle area.
            joystickInnerCircle.anchoredPosition = Vector2.ClampMagnitude(newPosition, joystickOuterCircle.sizeDelta.x / 2);
        }

        public void OnEndPositionUpdated(Vector2 position)
        {
            joystickOuterCircle.anchoredPosition = defaultJoystickPosition;
            joystickInnerCircle.anchoredPosition = Vector2.zero;
        }

        //NOTE: This is based on the anchored configuration set on the outer joystick circle.
        public Vector2 GetLowerLeftCornerAnchoredPosition()
        {
            Vector2 lowerLeftCorner = screenResolution / 2;
            lowerLeftCorner *= -1;
            return lowerLeftCorner;
        }
    }
}

namespace PlanesRemake.Runtime.UI.Views
{
    using UnityEngine;

    public class TouchCotrolsView : BaseView
    {
        [SerializeField]
        private RectTransform joystickOuterCircle = null;

        [SerializeField]
        private RectTransform joystickInnerCircle = null;

        private Vector2 initialPosition = Vector2.zero;
        private Vector2 defaultJoystickPosition = Vector2.zero;

        #region Unity Methods

        protected void Start()
        {
            defaultJoystickPosition = joystickOuterCircle.anchoredPosition;
        }

        #endregion

        public void SetInitialPosition(Vector2 position)
        {
            Vector2 middleAchoredPosition = GetLowerLeftCornerAnchoredPosition() + position;
            joystickOuterCircle.anchoredPosition = middleAchoredPosition;
            joystickInnerCircle.anchoredPosition = Vector2.zero;
            initialPosition = position;
        }

        public void SetDragPosition(Vector2 position)
        {
            //NOTE: Since the inner circle is a child of the outer circle, to new position needs to be local, 
            //hense the subtraction.
            Vector2 newPosition = position - initialPosition;
            //NOTE: Clamping the magnitude to always remain withing the outer circle area.
            joystickInnerCircle.anchoredPosition = Vector2.ClampMagnitude(newPosition, joystickOuterCircle.sizeDelta.x / 2);
        }

        public void SetEndPosition(Vector2 position)
        {
            joystickOuterCircle.anchoredPosition = defaultJoystickPosition;
            joystickInnerCircle.anchoredPosition = Vector2.zero;
        }

        //NOTE: This is based ont he anchored configuration set on the outer joystick circle.
        public Vector2 GetLowerLeftCornerAnchoredPosition()
        {
            Vector2 lowerLeftCorner = new Vector2(Screen.width / 2, Screen.height / 2);
            lowerLeftCorner *= -1;
            return lowerLeftCorner;
        }
    }
}

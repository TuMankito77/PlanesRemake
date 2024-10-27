namespace PlanesRemake.Runtime.UI.Views
{
    using UnityEngine;

    public class TouchControlsView : BaseView
    {
        [SerializeField]
        private RectTransform joystickParent = null;

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

        public void OnInitialPositionUpdated(Vector2 position)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickParent, 
                position, 
                Canvas.worldCamera, 
                out Vector2 localPosition);
            joystickOuterCircle.anchoredPosition = localPosition;
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
    }
}

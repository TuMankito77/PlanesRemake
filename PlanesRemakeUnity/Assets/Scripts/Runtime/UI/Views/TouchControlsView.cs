namespace PlanesRemake.Runtime.UI.Views
{
    using UnityEngine;
    using UnityEngine.UI;

    public class TouchControlsView : BaseView
    {
        [SerializeField]
        private RectTransform joystickParent = null;

        [SerializeField]
        private RectTransform joystickOuterCircle = null;

        [SerializeField]
        private RectTransform joystickInnerCircle = null;

        [SerializeField]
        private Image joystickOuterCircleImage = null;

        [SerializeField]
        private Image joystickInnerCircleImage = null;

        [SerializeField, Range(0, 1)]
        private float joystickTransparencyWhenInUse = 1;

        [SerializeField, Range(0, 1)]
        private float joystickTransparencyWhenInRest = 0.3f;
        
        private Vector2 initialPosition = Vector2.zero;
        private Vector2 defaultJoystickPosition = Vector2.zero;

        #region Unity Methods

        protected void Start()
        {
            defaultJoystickPosition = joystickOuterCircle.anchoredPosition;
            SetJoystickVisualsTransparency(joystickTransparencyWhenInRest);
        }

        #endregion

        public void OnInitialPositionUpdated(Vector2 position)
        {
            SetJoystickVisualsTransparency(joystickTransparencyWhenInUse);
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
            SetJoystickVisualsTransparency(joystickTransparencyWhenInRest);
            joystickOuterCircle.anchoredPosition = defaultJoystickPosition;
            joystickInnerCircle.anchoredPosition = Vector2.zero;
        }

        private void SetJoystickVisualsTransparency(float alpha)
        {
            float alphaClamped = Mathf.Clamp01(alpha);
            
            Color joystickOuterCircleColorUpdated = new Color(
                joystickOuterCircleImage.color.r, 
                joystickOuterCircleImage.color.g, 
                joystickOuterCircleImage.color.b, 
                alphaClamped);

            Color joystickInnerCircleColorUpdated = new Color(
                joystickInnerCircleImage.color.r,
                joystickInnerCircleImage.color.g,
                joystickInnerCircleImage.color.b,
                alphaClamped);

            joystickOuterCircleImage.color = joystickOuterCircleColorUpdated;
            joystickInnerCircleImage.color = joystickInnerCircleColorUpdated;
        }
    }
}

namespace PlanesRemake.Editor.UI.Views
{
    using UnityEditor;
    using UnityEngine;
    
    using PlanesRemake.Runtime.UI.Views;

    [CustomEditor(typeof(TouchControlsView))]
    public class TouchControlsViewInspector : Editor
    {
        private RectTransform joystickInnerCircle = null;
        private RectTransform joystickOuterCircle = null;
        private Vector2 defaultJoystickPosition = Vector2.zero;
        private Vector2 ScreenResolution => Handles.GetMainGameViewSize();

        #region Unity Methods

        private void OnEnable()
        {
            SerializedProperty joystickInnerCircleSP = serializedObject.FindProperty("joystickInnerCircle");
            joystickInnerCircle = joystickInnerCircleSP.objectReferenceValue as RectTransform;
            SerializedProperty joystickOuterCircleSP = serializedObject.FindProperty("joystickOuterCircle");
            joystickOuterCircle = joystickOuterCircleSP.objectReferenceValue as RectTransform;
            UpdateJoystickVisuals();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                UpdateJoystickVisuals();
            }

        }

        #endregion

        private Vector2 GetLowerLeftCornerAnchoredPosition()
        {
            Vector2 lowerLeftCorner = ScreenResolution / 2;
            lowerLeftCorner *= -1;
            return lowerLeftCorner;
        }

        private void UpdateJoystickVisuals()
        {
            float innerJoystickScaleFactor = joystickInnerCircle.sizeDelta.magnitude / joystickOuterCircle.sizeDelta.magnitude;
            float screenResolutionScaleFactor = serializedObject.FindProperty("screenResolutionScaleFactor").floatValue;
            float joystickSize = ScreenResolution.magnitude * screenResolutionScaleFactor;
            float innerJoystickSize = joystickSize * innerJoystickScaleFactor;
            joystickOuterCircle.sizeDelta = new Vector2(joystickSize, joystickSize);
            joystickInnerCircle.sizeDelta = new Vector2(innerJoystickSize, innerJoystickSize);
            float horizontalDefaultPlace = serializedObject.FindProperty("horizontalDefaultPlace").floatValue;
            float verticalDefaultPlace = serializedObject.FindProperty("verticalDefaultPlace").floatValue;
            defaultJoystickPosition = GetLowerLeftCornerAnchoredPosition() +
                new Vector2(ScreenResolution.x * horizontalDefaultPlace, ScreenResolution.y * verticalDefaultPlace);
            joystickOuterCircle.anchoredPosition = defaultJoystickPosition;
        }
    }
}

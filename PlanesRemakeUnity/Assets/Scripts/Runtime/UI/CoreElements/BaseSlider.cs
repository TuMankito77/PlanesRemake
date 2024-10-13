namespace PlanesRemake.Runtime.UI.CoreElements
{
    using System;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class BaseSlider : SelectableElement
    {
        public event Action<float> OnValueChanged = null;

        [SerializeField]
        private Slider slider = null;

        public void UpdateSliderValue(float newValue)
        {
            slider.value = Mathf.Clamp01(newValue);
        }

        protected override void CheckNeededComponents()
        {
            base.CheckNeededComponents();
            AddComponentIfNotFound(ref slider);
        }

        protected override void OnPointerUp(BaseEventData baseEventData)
        {
            base.OnPointerUp(baseEventData);
            OnValueChanged?.Invoke(slider.value);
        }
    }
}

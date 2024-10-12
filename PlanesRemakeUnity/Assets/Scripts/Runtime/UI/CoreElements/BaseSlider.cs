namespace PlanesRemake.Runtime.UI.CoreElements
{
    using UnityEngine;
    using UnityEngine.UI;

    public class BaseSlider : SelectableElement
    {
        [SerializeField]
        private Slider slider = null;

        protected override void CheckNeededComponents()
        {
            base.CheckNeededComponents();
            AddComponentIfNotFound(ref slider);
        }
    }
}

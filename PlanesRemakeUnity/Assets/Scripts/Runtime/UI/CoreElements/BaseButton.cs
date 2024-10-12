namespace PlanesRemake.Runtime.UI.CoreElements
{
    using System;

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class BaseButton : SelectableElement
    {
        public event Action onButtonPressed = null;

        [SerializeField]
        private Button button = null;

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            button.onClick.AddListener(OnButtonPressed);
        }

        #endregion

        protected override void CheckNeededComponents()
        {
            base.CheckNeededComponents();
            AddComponentIfNotFound(ref button);
        }

        protected override void OnSubmit(BaseEventData baseEventData)
        {
            Debug.LogWarning($"{gameObject.name}-Pointer Click.");
            //NOTE: Buttons stay on the selected state for some reason, and we are deselecting them as soon as it happens
            button.OnDeselect(baseEventData);
        }

        private void OnButtonPressed()
        {
            onButtonPressed?.Invoke();
        }
    }
}

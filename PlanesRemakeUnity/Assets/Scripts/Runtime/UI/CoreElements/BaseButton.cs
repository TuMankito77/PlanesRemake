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

        private ISeletableElementAnimator buttonAniator = null;

        #region Unity Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            button.onClick.AddListener(OnButtonPressed);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if(buttonAniator != null)
            {
                buttonAniator.OnSubmitAnimationCompleted -= onButtonPressed;
            }
            else
            {
                button.onClick.RemoveListener(OnButtonPressed);
            }
        }

        #endregion

        public void SetAnimationExternalModule(ISeletableElementAnimator sourceButtonAnimator)
        {
            buttonAniator = sourceButtonAnimator;
            button.onClick.RemoveListener(OnButtonPressed);
            buttonAniator.OnSubmitAnimationCompleted += OnButtonPressed;
        }

        protected override void CheckNeededComponents()
        {
            base.CheckNeededComponents();
            AddComponentIfNotFound(ref button);
        }

        protected override void OnSubmit(BaseEventData baseEventData)
        {
            base.OnSubmit(baseEventData);
            //NOTE: Buttons stay on the selected state for some reason, and we are deselecting them as soon as it happens
            button.OnDeselect(baseEventData);
        }

        protected override void OnPointerExit(BaseEventData baseEventData)
        {
            base.OnPointerExit(baseEventData);
            button.OnDeselect(baseEventData);
        }

        private void OnButtonPressed()
        {
            onButtonPressed?.Invoke();
        }
    }
}

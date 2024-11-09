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
            onSubmit += OnButtonPressed;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if(buttonAniator != null)
            {
                buttonAniator.OnSubmitAnimationCompleted -= onButtonPressed;
            }
        }

        #endregion

        public void SetAnimationExternalModule(ISeletableElementAnimator sourceButtonAnimator)
        {
            buttonAniator = sourceButtonAnimator;
            onSubmit -= OnButtonPressed;
            buttonAniator.OnSubmitAnimationCompleted += OnButtonPressed;
        }

        protected override void CheckNeededComponents()
        {
            base.CheckNeededComponents();
            AddComponentIfNotFound(ref button);
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

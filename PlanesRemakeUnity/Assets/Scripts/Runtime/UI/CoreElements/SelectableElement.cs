namespace PlanesRemake.Runtime.UI.CoreElements
{
    using System;
    
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class SelectableElement : MonoBehaviour
    {
        public event Action onSubmit = null;
        public event Action onSelect = null;
        public event Action onDeselect = null;

        [SerializeField]
        private EventTrigger eventTrigger = null;

        protected EventTriggerController eventTriggerController = null;
        private bool isSelected = false;
        private bool isSubscribedToInteractableEvents = false;

        #region Unity Methods

        protected virtual void Awake()
        {
            CheckNeededComponents();
            eventTriggerController = new EventTriggerController(eventTrigger);
        }

        protected virtual void OnEnable()
        {
            SubscribeToInteractableEvents();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeFromInteractableEvents();
        }

        protected virtual void OnDestroy()
        {
            
        }

        #endregion

        public void SetInteractable(bool isActive)
        {
            if(isActive && !isSubscribedToInteractableEvents)
            {
                SubscribeToInteractableEvents();
            }
            else if(!isActive && isSubscribedToInteractableEvents)
            {
                UnsubscribeFromInteractableEvents();
            }
        }

        protected virtual void CheckNeededComponents()
        {
            AddComponentIfNotFound(ref eventTrigger);
        }

        protected void AddComponentIfNotFound<T>(ref T componentReference) where T : UnityEngine.Component
        {
            if(componentReference != null)
            {
                return;
            }

            componentReference = GetComponent<T>();

            if(componentReference != null)
            {
                return;
            }

            componentReference = gameObject.AddComponent<T>();
        }

        protected virtual void OnPointerEnter(BaseEventData baseEventData)
        {
            eventTrigger.OnSelect(baseEventData);
        }

        protected virtual void OnPointerExit(BaseEventData baseEventData)
        {
            eventTrigger.OnDeselect(baseEventData);
        }

        protected virtual void OnSubmit(BaseEventData baseEventData)
        {
            onSubmit?.Invoke();
        }

        protected virtual void OnSelect(BaseEventData baseEventData)
        {
            if(!isSelected)
            {
                isSelected = true;
                onSelect?.Invoke();
            }    
        }

        protected virtual void OnDeselect(BaseEventData baseEventData)
        {
            if(isSelected)
            {
                isSelected = false;
                onDeselect?.Invoke();
            }
        }

        protected virtual void OnPointerDown(BaseEventData baseEventData)
        {

        }

        protected virtual void OnPointerUp(BaseEventData baseEventData)
        {

        }

        private void SubscribeToInteractableEvents()
        {
            isSubscribedToInteractableEvents = true;
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerEnter, OnPointerEnter);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerExit, OnPointerExit);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerClick, OnSubmit);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.Submit, OnSubmit);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.Select, OnSelect);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.Deselect, OnDeselect);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerDown, OnPointerDown);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerUp, OnPointerUp);
        }

        private void UnsubscribeFromInteractableEvents()
        {
            isSubscribedToInteractableEvents = false;
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerEnter, OnPointerEnter);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerExit, OnPointerExit);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerClick, OnSubmit);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.Submit, OnSubmit);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.Select, OnSelect);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.Deselect, OnDeselect);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerDown, OnPointerDown);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerUp, OnPointerUp);
        }
    }
}

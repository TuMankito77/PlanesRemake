namespace PlanesRemake.Runtime.UI.CoreElements
{
    using PlanesRemake.Runtime.Utils;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class SelectableElement : MonoBehaviour
    {
        [SerializeField]
        private EventTrigger eventTrigger = null;

        protected EventTriggerController eventTriggerController = null;
        
        #region Unity Methods

        protected virtual void Awake()
        {
            CheckNeededComponents();
            eventTriggerController = new EventTriggerController(eventTrigger);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerEnter, OnPointerEnter);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerExit, OnPointerExit);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerClick, OnSubmit);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.Submit, OnSubmit);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.Select, OnSelect);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.Deselect, OnDeselect);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerDown, OnPointerDown);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerUp, OnPointerUp);
        }

        #endregion

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
            
        }

        protected virtual void OnPointerExit(BaseEventData baseEventData)
        {
            
        }

        protected virtual void OnSubmit(BaseEventData baseEventData)
        {
            
        }

        protected virtual void OnSelect(BaseEventData baseEventData)
        {
            
        }

        protected virtual void OnDeselect(BaseEventData baseEventData)
        {
            
        }

        protected virtual void OnPointerDown(BaseEventData baseEventData)
        {

        }

        protected virtual void OnPointerUp(BaseEventData baseEventData)
        {

        }
    }
}

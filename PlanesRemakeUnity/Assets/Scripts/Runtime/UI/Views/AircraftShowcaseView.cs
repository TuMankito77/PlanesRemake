namespace PlanesRemake.Runtime.UI.Views
{
    using System;
    
    using UnityEngine;
    
    using PlanesRemake.Runtime.UI.WorldElements;
    using PlanesRemake.Runtime.Gameplay;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Utils;

    public class AircraftShowcaseView : BaseView, IListener
    {
        [SerializeField]
        private AircraftShowcaseStudio aircraftShowcaseStudioPrefab = null;

        [SerializeField]
        private AircraftDatabase aircraftDatabase = null;

        private AircraftShowcaseStudio aircraftShowcaseStudio = null;

        #region IListener

        public void HandleEvent(IComparable eventName, object data)
        {
            switch(eventName)
            {
                case UiEvents uiEvent:
                    {
                        HandleUiEvents(uiEvent, data);
                        break;
                    }

                default:
                    {
                        LoggerUtil.LogError($"{GetType()} - The event {eventName} is not handled by this class. You may need to unsubscribe.");
                        break;
                    }
            }
        }

        #endregion

        public override void TransitionIn(int sourceInteractableGroupId)
        {
            base.TransitionIn(sourceInteractableGroupId);
            EventDispatcher.Instance.AddListener(this, typeof(UiEvents));
            aircraftShowcaseStudio = Instantiate(aircraftShowcaseStudioPrefab);
            DontDestroyOnLoad(aircraftShowcaseStudio);
        }

        public override void TransitionOut()
        {
            base.TransitionOut();
            EventDispatcher.Instance.RemoveListener(this, typeof(UiEvents));
            Destroy(aircraftShowcaseStudio.gameObject);
        }

        private void HandleUiEvents(UiEvents uiEvent, object data)
        {
            switch(uiEvent)
            {
                case UiEvents.OnSetShowcaseAircraft:
                    {
                        string aircraftId = data as string;
                        AircraftData aircraftData = aircraftDatabase.GetFile(aircraftId);
                        aircraftShowcaseStudio.UpdateAircraftDisplayed(aircraftData.ShowcaseAircraftPrefab);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
    }
}


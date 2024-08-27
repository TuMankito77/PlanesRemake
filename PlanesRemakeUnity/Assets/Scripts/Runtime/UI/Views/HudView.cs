namespace PlanesRemake.Runtime.UI.Views
{
    using System;
    
    using UnityEngine;
    using UnityEngine.UI;
    
    using PlanesRemake.Runtime.Events;

    public class HudView : BaseView, IListener
    {
        [SerializeField]
        private Text coinsTextComponent = null;

        [SerializeField]
        private Text wallsTextComponent = null;

        public override void Initialize(Camera uiCamera)
        {
            base.Initialize(uiCamera);
            EventDispatcher.Instance.AddListener(this, typeof(UiEvents));
        }

        public override void Dispose()
        {
            base.Dispose();
            EventDispatcher.Instance.RemoveListener(this, typeof(UiEvents));
        }

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
                    break;
                }
            }
        }

        #endregion

        private void HandleUiEvents(UiEvents uiEvent, object data)
        {
            switch(uiEvent)
            {
                case UiEvents.OnCoinsValueChanged:
                {
                    string coinsValueUpdated = data as string;
                    coinsTextComponent.text = coinsValueUpdated;
                    break;
                }

                case UiEvents.OnWallsValueChanged:
                {
                    string wallsValueUpdated = data as string;
                    wallsTextComponent.text = wallsValueUpdated;
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

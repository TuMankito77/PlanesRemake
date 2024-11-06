namespace PlanesRemake.Runtime.Gameplay
{
    using System;
    
    using UnityEngine;

    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Gameplay.CommonBehaviors;
    using PlanesRemake.Runtime.Utils;

    public class RotatingBackground : MonoBehaviour, IListener
    {
        [SerializeField]
        private DirectionalSpinning directionSpinning = null;

        #region Unity Methods

        private void Start()
        {
            EventDispatcher.Instance.AddListener(this, typeof(UiEvents), typeof(GameplayEvents));
        }

        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener(this, typeof(UiEvents), typeof(GameplayEvents));
        }

        #endregion

        #region IListener

        public void HandleEvent(IComparable eventName, object data)
        {
            switch(eventName)
            {
                case UiEvents uiEvent:
                    {
                        HandleUiEvents(uiEvent);
                        break;
                    }

                case GameplayEvents gameplayEvent:
                    {
                        HandleGameplayEvents(gameplayEvent);
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

        private void HandleUiEvents(UiEvents uiEvent)
        {
            switch(uiEvent)
            {
                case UiEvents.OnPauseButtonPressed:
                    {
                        directionSpinning.enabled = false;
                        break;
                    }

                case UiEvents.OnUnpauseButtonPressed:
                    {
                        directionSpinning.enabled = true;
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        private void HandleGameplayEvents(GameplayEvents gameplayEvent)
        {
            switch(gameplayEvent)
            {
                case GameplayEvents.OnWallcollision:
                    {
                        directionSpinning.enabled = false;
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

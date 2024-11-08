namespace PlanesRemake.Runtime.UI.Views
{
    using System;
    
    using UnityEngine;
    using UnityEngine.UI;
    
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Sound;
    using PlanesRemake.Runtime.Utils;

    public class HudView : BaseView, IListener
    {
        [SerializeField]
        private Text coinsTextComponent = null;

        [SerializeField]
        private Text wallsTextComponent = null;

        [SerializeField]
        private Image fuelBar = null;

        private Timer fuelTimer = null;

        public override void Initialize(Camera uiCamera, AudioManager audioManager)
        {
            base.Initialize(uiCamera, audioManager);
            EventDispatcher.Instance.AddListener(this, typeof(UiEvents), typeof(GameplayEvents));
        }

        public override void Dispose()
        {
            base.Dispose();
            EventDispatcher.Instance.RemoveListener(this, typeof(UiEvents), typeof(GameplayEvents));
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

                case GameplayEvents gameplayEvent:
                {
                    HandleGameplayEvents(gameplayEvent, data);
                    break;
                }

                default:
                {
                    break;
                }
            }
        }

        #endregion

        private void HandleGameplayEvents(GameplayEvents gameplayEvent, object data)
        {
            switch(gameplayEvent)
            {
                case GameplayEvents.OnFuelCollected:
                {
                    fuelTimer.Restart();
                    break;
                }

                case GameplayEvents.OnWallcollision:
                {
                    fuelTimer.Stop();
                    break;
                }
            }
        }

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

                case UiEvents.OnSetFuelTimerDuration:
                {
                    if(fuelTimer != null)
                    {
                        fuelTimer.OnTimerTick -= OnFuelTimerTick;
                        fuelTimer.OnTimerCompleted -= OnFuelTimerCompleted;
                    }

                    float duration = (float)data;
                    fuelTimer = new Timer(duration);
                    fuelTimer.OnTimerTick += OnFuelTimerTick;
                    fuelTimer.OnTimerCompleted += OnFuelTimerCompleted;
                    fuelTimer.Start();
                    break;
                }

                case UiEvents.OnPauseButtonPressed:
                {
                    fuelTimer.Pause();
                    break;
                }

                case UiEvents.OnUnpauseButtonPressed:
                {
                    fuelTimer.Start();
                    break;
                }

                default:
                {
                    break;
                }
            }
        }

        private void OnFuelTimerCompleted()
        {
            fuelTimer.OnTimerTick -= OnFuelTimerTick;
            fuelTimer.OnTimerCompleted -= OnFuelTimerCompleted;
            EventDispatcher.Instance.Dispatch(GameplayEvents.OnFuelEmpty);
        }

        private void OnFuelTimerTick(float deltaTime, float timeTranscurred)
        {
            fuelBar.fillAmount = Mathf.Clamp01(1 - (timeTranscurred / fuelTimer.Duration));
        }
    }
}

namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using System;
    using System.Collections.Generic;
    
    using UnityEngine;
    
    using PlanesRemake.Runtime.Utils;
    using PlanesRemake.Runtime.Events;

    public abstract class BaseAbility : IListener
    {
        public event Action onAbilityEffectFinished;

        protected GameObject owner = null;
        protected Timer activeTimer = null;
        protected AbilityData abilityData = null;
        protected List<Type> eventsToListenFor = null;

        protected abstract bool IsAbilityTimerTickEnabled { get; }

        public BaseAbility(GameObject sourceOwner, AbilityData sourceAbilityData)
        {
            owner = sourceOwner;
            abilityData = sourceAbilityData;
            activeTimer = new Timer(abilityData.Duration);
            eventsToListenFor = new List<Type>();
            eventsToListenFor.Add(typeof(UiEvents));
        }

        public virtual void Activate()
        {
            EventDispatcher.Instance.AddListener(this, eventsToListenFor.ToArray());
            if(activeTimer.IsRunning)
            {
                activeTimer.Restart();
                return;
            }

            activeTimer.OnTimerCompleted += Deactivate;
            activeTimer.OnTimerCompleted += onAbilityEffectFinished;

            if(IsAbilityTimerTickEnabled)
            {
                activeTimer.OnTimerTick += OnAbilityTimerTick;
            }

            activeTimer.Start();
        }

        public virtual void Deactivate()
        {
            EventDispatcher.Instance.RemoveListener(this, eventsToListenFor.ToArray());
            activeTimer.Stop();
            activeTimer.OnTimerCompleted -= Deactivate;

            if(IsAbilityTimerTickEnabled)
            {
                activeTimer.OnTimerTick -= OnAbilityTimerTick;
            }
        }

        protected virtual void OnAbilityTimerTick(float deltaTime, float timeTranscurred)
        {

        }

        public virtual void HandleEvent(IComparable eventName, object data)
        {
            if(!eventsToListenFor.Contains(eventName.GetType()))
            {
                LoggerUtil.LogError($"{GetType()} - The event {eventName} is not handled by this class. You may need to unsubscribe.");    
                return;
            }

            switch(eventName)
            {
                case UiEvents uiEvent:
                    {
                        HandleUiEvents(uiEvent);
                        break;
                    }
            }
        }

        private void HandleUiEvents(UiEvents uiEvent)
        {
            switch(uiEvent)
            {
                case UiEvents.OnPauseButtonPressed:
                    {
                        activeTimer.Pause();
                        break;
                    }

                case UiEvents.OnUnpauseButtonPressed:
                    {
                        activeTimer.Start();
                        break;
                    }
            }
        }
    }
}

namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using System;
    
    using UnityEngine;
    
    using PlanesRemake.Runtime.Utils;

    public abstract class BaseAbility
    {
        public event Action onAbilityEffectFinished;

        protected GameObject owner = null;
        protected Timer activeTimer = null;
        protected AbilityData abilityData = null;

        protected abstract bool IsAbilityTimerTickEnabled { get; }

        public BaseAbility(GameObject sourceOwner, AbilityData sourceAbilityData)
        {
            owner = sourceOwner;
            abilityData = sourceAbilityData;
            activeTimer = new Timer(abilityData.Duration);
        }

        public virtual void Activate()
        {
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
    }
}

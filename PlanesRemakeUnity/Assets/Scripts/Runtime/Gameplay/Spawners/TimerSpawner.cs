namespace PlanesRemake.Runtime.Gameplay.Spawners
{
    using System;
    
    using UnityEngine;

    using PlanesRemake.Runtime.Utils;
    using PlanesRemake.Runtime.Events;

    public abstract class TimerSpawner : BaseSpawner, IListener
    {
        private event Func<float> GetSpawningDelayInSeconds;
        
        protected CameraBoundaries boundaries = default(CameraBoundaries);

        private Timer spawningTimer = null; 
        
        protected abstract Vector3 StartingPosition { get; }
        protected abstract Quaternion StartingRotation { get; }

        #region IListener

        public virtual void HandleEvent(IComparable eventName, object data)
        {
            switch (eventName)
            {
                case GameplayEvents gameplayEvent:
                    {
                        HandleGameplayEvents(gameplayEvent, data);
                        break;
                    }

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

        public TimerSpawner(BasePoolableObject sourcePrefab, int objectPoolSize, int objectPoolMaxCapacity, Camera sourceIsometricCamera, CameraBoundaries cameraBoundariesOffset, Func<float> getSpawningDelayInSeconds) 
            : base(sourcePrefab, objectPoolSize, objectPoolMaxCapacity)
        {
            boundaries = sourceIsometricCamera.GetScreenBoundariesInWorld(Vector3.zero);
            boundaries.AddOffset(cameraBoundariesOffset);
            GetSpawningDelayInSeconds = getSpawningDelayInSeconds;
            spawningTimer = new Timer(GetSpawningDelayInSeconds(), sourceIsRepeating: false);
            spawningTimer.OnTimerCompleted += OnSpawningTimerCompleted;
            spawningTimer.Start();
            EventDispatcher.Instance.AddListener(this, typeof(GameplayEvents), typeof(UiEvents));
        }

        public override void Dispose()
        {
            spawningTimer.OnTimerCompleted -= OnSpawningTimerCompleted;
            spawningTimer.Stop();
            base.Dispose();
        }

        protected override BasePoolableObject OnCreatePoolObject()
        {
            return GameObject.Instantiate(prefab, StartingPosition, StartingRotation, prefabInstancesContainer.transform);
        }

        protected override void OnReleasePoolObject(BasePoolableObject instance)
        {
            base.OnReleasePoolObject(instance);
            instance.transform.position = StartingPosition;
            instance.transform.rotation = StartingRotation;
        }

        protected override void OnGetPoolObject(BasePoolableObject instance)
        {
            base.OnGetPoolObject(instance);
            instance.transform.position = StartingPosition;
            instance.transform.rotation = StartingRotation;
        }

        private void HandleGameplayEvents(GameplayEvents gameplayEvent, object data)
        {
            switch (gameplayEvent)
            {
                case GameplayEvents.OnWallcollision:
                    {
                        spawningTimer.Pause();
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        private void HandleUiEvents(UiEvents uiEvent, object data)
        {
            switch (uiEvent)
            {
                case UiEvents.OnPauseButtonPressed:
                    {
                        spawningTimer.Pause();
                        break;
                    }

                case UiEvents.OnUnpauseButtonPressed:
                    {
                        spawningTimer.Start();
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        private void OnSpawningTimerCompleted()
        {
            prefabInstancesPool.Get();
            spawningTimer.OnTimerCompleted -= OnSpawningTimerCompleted;
            spawningTimer = new Timer(GetSpawningDelayInSeconds(), sourceIsRepeating: false);
            spawningTimer.OnTimerCompleted += OnSpawningTimerCompleted;
            spawningTimer.Start();
        }
    }
}

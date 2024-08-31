namespace PlanesRemake.Runtime.Gameplay.Spawners
{
    using System;
    
    using UnityEngine;
    
    using PlanesRemake.Runtime.Utils;
    using PlanesRemake.Runtime.Events;

    public abstract class BaseSpawner : IListener
    {
        protected CameraExtensions.Boundaries boundaries = default(CameraExtensions.Boundaries);

        private GameObject prefab = null;
        private Timer spawningTimer = null;

        protected abstract Vector3 StartingPosition { get;}
        protected abstract Quaternion StartingRotation { get;}
        protected abstract float SpawnDelayInSeconds { get;}
        protected abstract bool SpawnPrefabOnCreation { get;}

        #region IListener

        public void HandleEvent(IComparable eventName, object data)
        {
            switch(eventName)
            {
                case GameplayEvents gameplayEvent:
                {
                    HandleGameplayEvents(gameplayEvent, data);
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

        public BaseSpawner(GameObject sourcePrefab, Camera sourceIsometricCamera)
        {
            prefab = sourcePrefab;

            boundaries = sourceIsometricCamera.GetScreenBoundariesInWorld(Vector3.zero);

            //NOTE: Maybe this should be moved to the class that needs this behavior rather than leaving it here.
            if(SpawnPrefabOnCreation)
            {
                OnSpawningTimerCompleted();
            }

            spawningTimer = new Timer(SpawnDelayInSeconds, sourceIsRepeating: true);
            spawningTimer.OnTimerCompleted += OnSpawningTimerCompleted;
            spawningTimer.Start();

            EventDispatcher.Instance.AddListener(this, typeof(GameplayEvents));
        }

        public virtual void Dispose()
        {
            spawningTimer.OnTimerCompleted -= OnSpawningTimerCompleted;
            spawningTimer.Stop();
        }

        protected virtual GameObject SpawnPrefab(GameObject prefab)
        {
            return GameObject.Instantiate(prefab, StartingPosition, StartingRotation);
        }

        private void OnSpawningTimerCompleted()
        {
            SpawnPrefab(prefab);
        }

        private void HandleGameplayEvents(GameplayEvents gameplayEvent, object data)
        {
            switch(gameplayEvent)
            {
                case GameplayEvents.OnWallcollision:
                {
                    spawningTimer.Pause();
                    break;
                }
            }
        }
    }
}

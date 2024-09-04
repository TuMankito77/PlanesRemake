namespace PlanesRemake.Runtime.Gameplay.Spawners
{
    using System;

    using UnityEngine;
    using UnityEngine.Pool;

    using PlanesRemake.Runtime.Utils;
    using PlanesRemake.Runtime.Events;

    public abstract class BaseSpawner : IListener
    {
        protected CameraExtensions.Boundaries boundaries = default(CameraExtensions.Boundaries);
        protected IObjectPool<GameObject> prefabInstancesPool = null;

        private GameObject prefab = null;
        private GameObject prefabInstancesContainer = null;
        private Timer spawningTimer = null;

        protected abstract Vector3 StartingPosition { get; }
        protected abstract Quaternion StartingRotation { get; }
        protected abstract float SpawnDelayInSeconds { get; }
        protected abstract bool SpawnPrefabOnCreation { get; }

        #region IListener

        public void HandleEvent(IComparable eventName, object data)
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

        public BaseSpawner(GameObject sourcePrefab, int objectPoolSize, int objectPoolMaxCapacity, Camera sourceIsometricCamera)
        {
            prefab = sourcePrefab;
            prefabInstancesContainer = new GameObject($"{prefab.name}_Container");
            prefabInstancesPool = new ObjectPool<GameObject>(OnCreatePoolObject, OnGetPoolObject, OnReleasePoolObject, OnDestroyPoolObject, collectionCheck: true, objectPoolSize, objectPoolMaxCapacity);

            boundaries = sourceIsometricCamera.GetScreenBoundariesInWorld(Vector3.zero);

            //NOTE: Maybe this should be moved to the class that needs this behavior rather than leaving it here.
            if (SpawnPrefabOnCreation)
            {
                prefabInstancesPool.Get();
            }

            spawningTimer = new Timer(SpawnDelayInSeconds, sourceIsRepeating: true);
            spawningTimer.OnTimerCompleted += OnSpawningTimerCompleted;
            spawningTimer.Start();

            EventDispatcher.Instance.AddListener(this, typeof(GameplayEvents), typeof(UiEvents));
        }

        public virtual void Dispose()
        {
            spawningTimer.OnTimerCompleted -= OnSpawningTimerCompleted;
            spawningTimer.Stop();
            prefabInstancesPool.Clear();
        }

        protected virtual GameObject OnCreatePoolObject()
        {
            return GameObject.Instantiate(prefab, StartingPosition, StartingRotation, prefabInstancesContainer.transform);
        }

        protected virtual void OnGetPoolObject(GameObject instance)
        {
            instance.SetActive(true);
        }

        protected virtual void OnReleasePoolObject(GameObject instance)
        {
            instance.SetActive(false);
            instance.transform.position = StartingPosition;
            instance.transform.rotation = StartingRotation;
        }

        protected virtual void OnDestroyPoolObject(GameObject instance)
        {

        }

        private void OnSpawningTimerCompleted()
        {
            prefabInstancesPool.Get();
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
            switch(uiEvent)
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
    }
}

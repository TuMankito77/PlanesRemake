namespace PlanesRemake.Runtime.Gameplay
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Utils;
    
    public abstract class BaseSpawner
    {
        protected CameraExtensions.Boundaries boundaries = default(CameraExtensions.Boundaries);

        private GameObject prefab = null;
        private Timer spawningTimer = null;

        protected abstract Vector3 StartingPosition { get;}
        protected abstract Quaternion StartingRotation { get;}
        protected abstract float SpawnDelayInSeconds { get;}
        protected abstract bool SpawnPrefabOnCreation { get;}

        public BaseSpawner(GameObject sourcePrefab, Camera sourceIsometricCamera)
        {
            prefab = sourcePrefab;

            boundaries = sourceIsometricCamera.GetScreenBoundariesInWorld(Vector3.zero);

            if(SpawnPrefabOnCreation)
            {
                OnSpawningTimerCompleted();
            }

            spawningTimer = new Timer(SpawnDelayInSeconds, sourceIsRepeating: true);
            spawningTimer.OnTimerCompleted += OnSpawningTimerCompleted;
            spawningTimer.Start();
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
    }
}

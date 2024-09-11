namespace PlanesRemake.Runtime.Gameplay.Spawners
{
    using UnityEngine;
    using UnityEngine.Pool;

    public abstract class BaseSpawner
    {
        protected IObjectPool<BasePoolableObject> prefabInstancesPool = null;

        protected BasePoolableObject prefab = null;
        protected GameObject prefabInstancesContainer = null;

        public BaseSpawner(BasePoolableObject sourcePrefab, int objectPoolSize, int objectPoolMaxCapacity)
        {
            prefab = sourcePrefab;
            prefabInstancesContainer = new GameObject($"{prefab.name}_Container");
            prefabInstancesPool = new ObjectPool<BasePoolableObject>(OnCreatePoolObject, OnGetPoolObject, OnReleasePoolObject, OnDestroyPoolObject, collectionCheck: true, objectPoolSize, objectPoolMaxCapacity);
        }

        public virtual void Dispose()
        {
            prefabInstancesPool.Clear();
        }

        protected virtual BasePoolableObject OnCreatePoolObject()
        {
            return GameObject.Instantiate(prefab, prefabInstancesContainer.transform);
        }

        protected virtual void OnGetPoolObject(BasePoolableObject instance)
        {
            instance.gameObject.SetActive(true);
        }

        protected virtual void OnReleasePoolObject(BasePoolableObject instance)
        {
            instance.gameObject.SetActive(false);
        }

        protected virtual void OnDestroyPoolObject(BasePoolableObject instance)
        {

        }
    }
}

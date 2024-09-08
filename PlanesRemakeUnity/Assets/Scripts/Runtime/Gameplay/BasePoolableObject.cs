namespace PlanesRemake.Runtime.Gameplay
{
    using UnityEngine;
    using UnityEngine.Pool;

    public abstract class BasePoolableObject : MonoBehaviour
    {
        protected abstract IObjectPool<BasePoolableObject> ObjectPool { get; }

        public virtual void ReleaseObject()
        {
            ObjectPool.Release(this);
        }
    }
}

namespace PlanesRemake.Runtime.Gameplay
{
    using UnityEngine.Pool;
    
    public class TimerPoolableObject : BasePoolableObject
    {
        private IObjectPool<BasePoolableObject> objectPool = null;
        
        protected override IObjectPool<BasePoolableObject> ObjectPool => objectPool;

        public void Initialize(IObjectPool<BasePoolableObject> sourceObjectPool)
        {
            objectPool = sourceObjectPool;
        }
    }
}

namespace PlanesRemake.Runtime.Gameplay.Spawners
{
    using System;
    using System.Collections.Generic;

    using PlanesRemake.Runtime.Events;

    public abstract class EventDrivenSpawner : BaseSpawner, IListener
    {
        protected abstract Dictionary<Type, Action<IComparable, object>> ActionPerEventType { get; }
        List<Type> eventsToListenTo = null;

        public EventDrivenSpawner(BasePoolableObject sourcePrefab, int objectPoolSize, int objectPoolMaxCapacity)
            : base(sourcePrefab, objectPoolSize, objectPoolMaxCapacity)
        {
            eventsToListenTo = new List<Type>();

            foreach (Type eventType in ActionPerEventType.Keys)
            {
                eventsToListenTo.Add(eventType);
            }

            EventDispatcher.Instance.AddListener(this, eventsToListenTo.ToArray());
        }

        public override void Dispose()
        {
            base.Dispose();
            EventDispatcher.Instance.RemoveListener(this, eventsToListenTo.ToArray());
        }

        #region IListener

        public virtual void HandleEvent(IComparable eventName, object data)
        {
            if(ActionPerEventType.TryGetValue(eventName.GetType(), out Action<IComparable, object> HandleEventPerType))
            {
                HandleEventPerType?.Invoke(eventName, data);
            }
        }

        #endregion
    }
}


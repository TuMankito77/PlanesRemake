namespace PlanesRemastered.Runtime.Events
{
    using System;
    using System.Collections.Generic;

    public class EventDispatcher
    {
        #region Singleton

        private static EventDispatcher instance = null;
        
        public static EventDispatcher Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new EventDispatcher();
                }

                return instance;
            }
        }

        #endregion

        private bool isDispatchingEvent = false;
        private Dictionary<Type, List<IListener>> listenersPerType = null;
        private Queue<IComparable> eventsQueued = null;

        public EventDispatcher()
        {
            listenersPerType = new Dictionary<Type, List<IListener>>();
            isDispatchingEvent = false;
            eventsQueued = new Queue<IComparable>();
        }

        public void AddListener(IListener listener, params Type[] eventTypes)
        {
            foreach(Type eventType in eventTypes)
            {
                List<IListener> listeners = null;

                if(listenersPerType.TryGetValue(eventType, out listeners) && !listeners.Contains(listener))
                {
                    listeners.Add(listener);
                    continue;
                }

                listeners = new List<IListener>()
                {
                    listener
                };

                listenersPerType.Add(eventType, listeners);
            }
        }

        public void RemoveListener(IListener listener, params Type[] eventTypes)
        {
            foreach (Type eventType in eventTypes)
            {
                List<IListener> listeners = null;

                if (listenersPerType.TryGetValue(eventType, out listeners) && listeners.Contains(listener))
                {
                    listeners.Remove(listener);
                    continue;
                }
            }
        }

        public void Dispatch(IComparable eventName, object data = null)
        {
            Type eventType = eventName.GetType();

            if(!listenersPerType.TryGetValue(eventType, out List<IListener> listeners) || listeners.Count <= 0)
            {
                return;
            }

            if(isDispatchingEvent)
            {
                eventsQueued.Enqueue(eventName);
                return;
            }

            isDispatchingEvent = true;

            foreach(IListener listener in listeners)
            {
                //To-do: Send the data by allowing the senders to send an event request that will contain data
                listener.HandleEvent(eventName, data);
            }

            isDispatchingEvent = false;

            if(eventsQueued.Count > 0)
            {
                Dispatch(eventsQueued.Dequeue()); 
            }
        }
    }
}
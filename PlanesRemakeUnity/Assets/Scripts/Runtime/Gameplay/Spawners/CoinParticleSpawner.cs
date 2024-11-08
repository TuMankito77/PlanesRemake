namespace PlanesRemake.Runtime.Gameplay.Spawners
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Utils;

    public class CoinParticleSpawner : EventDrivenSpawner
    {
        private Dictionary<Type, Action<IComparable, object>> actionPerEventType = null;

        protected override Dictionary<Type, Action<IComparable, object>> ActionPerEventType
        {
            get
            {
                if(actionPerEventType == null)
                {
                    actionPerEventType = new Dictionary<Type, Action<IComparable, object>>()
                    {
                        { typeof(GameplayEvents), (eventName, data) => { HandleGameplayEvents((GameplayEvents)eventName, data); } }
                    };
                }
                return actionPerEventType;
            }
        }

        public CoinParticleSpawner(TimerPoolableObject sourcePrefab, int objectPoolSize, int objectPoolMaxCapacity) 
            : base(sourcePrefab, objectPoolSize, objectPoolMaxCapacity)
        {

        }

        protected override void OnGetPoolObject(BasePoolableObject instance)
        {
            base.OnGetPoolObject(instance);
            instance.GetComponent<TimerPoolableObject>().Initialize(prefabInstancesPool);
        }

        private void SpawnParticles(Vector3 position)
        {
            BasePoolableObject vfx = prefabInstancesPool.Get();
            vfx.gameObject.transform.position = position;
            ParticleSystem component = vfx.GetComponent<ParticleSystem>();
            Timer durationTimer = new Timer(component.main.startLifetimeMultiplier);
            durationTimer.OnTimerCompleted += () => vfx.ReleaseObject();
            component.Play();
            durationTimer.Start();
        }

        public void HandleGameplayEvents(GameplayEvents gameplayEvent, object data)
        {
            switch(gameplayEvent)
            {
                case GameplayEvents.OnCoinCollected:
                    {
                        Coin coin = data as Coin;
                        SpawnParticles(coin.transform.position);
                        break;
                    }

                case GameplayEvents.OnFuelCollected:
                    {
                        Fuel fuel = data as Fuel;
                        SpawnParticles(fuel.transform.position);
                        break;
                    }
            }
        }
    }
}

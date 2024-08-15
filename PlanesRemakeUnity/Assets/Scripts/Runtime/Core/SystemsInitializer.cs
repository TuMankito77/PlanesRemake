namespace PlanesRemastered.Runtime.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UnityEngine;

    public class SystemsInitializer
    {
        public event Action OnSystemsInitialized = null;

        private Dictionary<Type, BaseSystem> systemsInitialized = null;

        public SystemsInitializer()
        {
            systemsInitialized = new Dictionary<Type, BaseSystem>();
        }

        public async void InitializeSystems(IEnumerable<BaseSystem> sourceSystemsToInitialize)
        {
            Dictionary<Type, BaseSystem> systemsToInitialize = new Dictionary<Type, BaseSystem>();

            foreach(BaseSystem baseSystem in sourceSystemsToInitialize)
            {
                systemsToInitialize.Add(baseSystem.GetType(), baseSystem);
            }
            
            List<Task> initializationTasks = new List<Task>();
            List<BaseSystem> systemsInitializing = new List<BaseSystem>();
            List<Type> dependenciesToVerify = new List<Type>(systemsToInitialize.Count);

            foreach (Type baseSystemType in systemsToInitialize.Keys)
            {
                Debug.Assert(systemsToInitialize.ContainsKey(baseSystemType), 
                    $"{GetType()}: You are adding a type of system more than once, you can only have one system per class type");
                dependenciesToVerify.Add(baseSystemType);
            }

            while(systemsToInitialize.Count > 0)
            {
                foreach(BaseSystem baseSystem in systemsToInitialize.Values)
                {
                    dependenciesToVerify.Remove(baseSystem.GetType());
                    bool isDepencyNeededToInitialize = false;

                    foreach(Type dependency in dependenciesToVerify)
                    {
                        if(baseSystem.Dependencies.ContainsKey(dependency))
                        {
                            isDepencyNeededToInitialize = true;
                            break;
                        }
                    }
                    
                    dependenciesToVerify.Add(baseSystem.GetType());

                    if(isDepencyNeededToInitialize)
                    {
                        continue;
                    }

                    systemsInitializing.Add(baseSystem);
                    initializationTasks.Add(baseSystem.Initialize(systemsInitialized.Values));
                }

                await Task.WhenAll(initializationTasks);

                foreach(BaseSystem systemInitiailized in systemsInitializing)
                {
                    Type systemType = systemInitiailized.GetType();
                    systemsToInitialize.Remove(systemType);
                    dependenciesToVerify.Remove(systemType);
                    systemsInitialized.Add(systemType, systemInitiailized);
                }

                systemsInitializing.Clear();
            }

            OnSystemsInitialized?.Invoke();
        }

        public T GetSystem<T>() where T : BaseSystem
        {
            Type systemType = typeof(T);
            return systemsInitialized.ContainsKey(systemType) ? systemsInitialized[systemType] as T : null;
        }
    }
}

namespace PlanesRemastered.Runtime.Core
{
    using System;

    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ContentLoader : BaseSystem
    {
        public void LoadScene(string scenePath, LoadSceneMode loadSceneMode, Action OnSceneLoaded, bool setAsMainScene = true)
        {
            AsyncOperation loadSceneAsyncOperation = SceneManager.LoadSceneAsync(scenePath, loadSceneMode);

            loadSceneAsyncOperation.completed += (asyncOperation) =>
            {
                if(setAsMainScene)
                {
                    Scene activeScene = SceneManager.GetSceneByName(scenePath);
                    SceneManager.SetActiveScene(activeScene);
                    DynamicGI.UpdateEnvironment();
                }   
                
                OnSceneLoaded?.Invoke();
            };
        }

        public void UnloadScene(string scenePath, Action OnSceneUnloaded)
        {
            AsyncOperation unloadSceneAsyncOperation = SceneManager.UnloadSceneAsync(scenePath);

            unloadSceneAsyncOperation.completed += (asyncOperation) =>
            {
                OnSceneUnloaded?.Invoke();
            };
        }

        public void LoadAsset<T>(string address, Action<T> onAssetLoaded, Action onFailedToLoadAsset) where T : UnityEngine.Object
        {
            ResourceRequest resourceRequest = Resources.LoadAsync(address);
            resourceRequest.completed += (asyncOperation) =>
            {
                if(resourceRequest.asset == null)
                {
                    onFailedToLoadAsset?.Invoke();
                }
                else
                {
                    T assetLoaded = resourceRequest.asset as T;

                    if(assetLoaded == null)
                    {
                        assetLoaded = (resourceRequest.asset as GameObject).GetComponent<T>();
                    }

                    onAssetLoaded(assetLoaded);
                }
            };
        }

        public void UnloadAsset(UnityEngine.Object asset)
        {
            Resources.UnloadAsset(asset);
        }
    }
}


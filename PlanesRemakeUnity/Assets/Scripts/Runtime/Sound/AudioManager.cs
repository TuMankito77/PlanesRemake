namespace PlanesRemake.Runtime.Sound
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using PlanesRemake.Runtime.Core;
    using UnityEngine;

    public class AudioManager : BaseSystem
    {
        private const string CLIPS_CONTAINER_SPRITABLE_OBJECT_PATH = "Sound/ClipsContainer";
        private const string AUDIO_PLAYER_PREFAB_PATH = "Sound/AudioPlayer";

        private AudioPlayer audioPlayer = null;
        private ClipsContanier clipsContainer = null;

        public override async Task<bool> Initialize(IEnumerable<BaseSystem> sourceDependencies)
        {
            await base.Initialize(sourceDependencies);
            AudioPlayer audioPlayerPrefab = await LoadAsset<AudioPlayer>(AUDIO_PLAYER_PREFAB_PATH);
            
            if(audioPlayerPrefab == null)
            {
                return false;
            }

            audioPlayer = GameObject.Instantiate(audioPlayerPrefab);
            audioPlayer.name = audioPlayerPrefab.name;
            GameObject.DontDestroyOnLoad(audioPlayer);

            clipsContainer = await LoadAsset<ClipsContanier>(CLIPS_CONTAINER_SPRITABLE_OBJECT_PATH);
            
            if(clipsContainer == null)
            {
                return false;
            }

            clipsContainer.Initialize();
            
            return true;
        }

        public void PlayGameplayClip(string clipId)
        {
            Debug.Assert(clipsContainer.ClipsById.ContainsKey(clipId),
                $"{GetType().Name} - The clip {clipId} id does not exist!");

            AudioClip audioClip = clipsContainer.ClipsById[clipId];
            audioPlayer.PlayGameplayClip(audioClip);
        }

        public void PauseGameplayClips()
        {
            audioPlayer.PauseGameplayAudioSource();
        }

        public void UnPauseGameplayClips()
        {
            audioPlayer.UnPauseGameplayAudioSource();
        }

        public void PlayGeneralClip(string clipId)
        {
            Debug.Assert(clipsContainer.ClipsById.ContainsKey(clipId),
                $"{GetType().Name} - The clip {clipId} id does not exist!");

            AudioClip audioClip = clipsContainer.ClipsById[clipId];
            audioPlayer.PlayGeneralClip(audioClip);
        }

        public void PuaseGeneralClips()
        {
            audioPlayer.PauseGeneralAudioSource();
        }

        public void UnPauseGeneralClips()
        {
            audioPlayer.PauseGeneralAudioSource();
        }

        public void PlayBackgroundMusic(string clipId)
        {
            Debug.Assert(clipsContainer.ClipsById.ContainsKey(clipId),
                $"{GetType().Name} - The clip {clipId} id does not exist!");

            AudioClip audioClip = clipsContainer.ClipsById[clipId];
            audioPlayer.PlayBackgroundMusic(audioClip);
        }

        public void PauseBackgroundMusic()
        {
            audioPlayer.PauseBackgroundMusic();
        }

        public void UnPaseBackgroundMusic()
        {
            audioPlayer.UnPauseBackgroundMusic();
        }

        private async Task<T> LoadAsset<T>(string address) where T : UnityEngine.Object
        {
            bool isLoadingAsset = true;
            ContentLoader contentLoader = GetDependency<ContentLoader>();
            T assetReference = null;
            contentLoader.LoadAsset<T>
                (address,
                (assetLoaded) =>
                {
                    assetReference = assetLoaded;
                    isLoadingAsset = false;
                },
                () => isLoadingAsset = false);

            while(isLoadingAsset)
            {
                await Task.Yield();
            }

            return assetReference;
        }
    }
}

namespace PlanesRemake.Runtime.Sound
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using PlanesRemake.Runtime.Core;

    public class AudioManager : BaseSystem
    {
        private const string CLIPS_CONTAINER_SPRITABLE_OBJECT_PATH = "Sound/ClipsContainer";
        private const string AUDIO_PLAYER_PREFAB_PATH = "Sound/AudioPlayer";

        private AudioPlayer audioPlayer = null;
        private ClipsContanier clipsContainer = null;

        public override async Task<bool> Initialize(IEnumerable<BaseSystem> sourceDependencies)
        {
            await base.Initialize(sourceDependencies);
            audioPlayer = await LoadAsset<AudioPlayer>(AUDIO_PLAYER_PREFAB_PATH);
            
            if(audioPlayer == null)
            {
                return false;
            }

            clipsContainer = await LoadAsset<ClipsContanier>(CLIPS_CONTAINER_SPRITABLE_OBJECT_PATH);
            
            if(clipsContainer == null)
            {
                return false;
            }

            clipsContainer.Initialize();
            
            return true;
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

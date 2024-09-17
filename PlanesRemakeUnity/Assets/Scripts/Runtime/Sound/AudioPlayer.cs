namespace PlanesRemake.Runtime.Sound
{
    using UnityEngine;

    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioSource gameplayAudioSource = null;

        [SerializeField]
        private AudioSource generalAudioSource = null;

        [SerializeField]
        private AudioSource backGroundMusicAudioSource = null;

        public void PlayGameplayClip(AudioClip clip)
        {
            gameplayAudioSource.PlayOneShot(clip);
        }

        public void PauseGameplayAudioSource()
        {
            gameplayAudioSource.Pause();
        }

        public void UnPauseGameplayAudioSource()
        {
            gameplayAudioSource.UnPause();
        }

        public void PlayGeneralClip(AudioClip clip)
        {
            generalAudioSource.PlayOneShot(clip);
        }

        public void PauseGeneralAudioSource()
        {
            generalAudioSource.Pause();
        }

        public void UnPauseGenericAudioSource()
        {
            generalAudioSource.UnPause();
        }

        public void PlayBackgroundMusic(AudioClip musicClip)
        {
            backGroundMusicAudioSource.clip = musicClip;
            backGroundMusicAudioSource.loop = true;
            backGroundMusicAudioSource.Play();
        }

        public void PauseBackgroundMusic()
        {
            backGroundMusicAudioSource.Pause();
        }

        public void UnPauseBackgroundMusic()
        {
            backGroundMusicAudioSource.UnPause();
        }
    }
}


namespace PlanesRemake.Runtime.UI.Views
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.UI.CoreElements;
    using PlanesRemake.Runtime.Events;

    public class OptionsMenuView : BaseView
    {
        [SerializeField]
        private BaseSlider musicSlider = null;

        [SerializeField]
        private BaseSlider vfxSlider = null;

        #region Unity Region

        protected override void Awake()
        {
            base.Awake();
        }

        #endregion

        public override void TransitionIn(int sourceInteractableGroupId)
        {
            base.TransitionIn(sourceInteractableGroupId);
            musicSlider.OnValueChanged += OnMusicVolumeChanged;
            vfxSlider.OnValueChanged += OnVfxVolumeChanged;
            musicSlider.UpdateSliderValue(audioManager.CurrentMusicVolume);
            vfxSlider.UpdateSliderValue(audioManager.CurrentVfxVolume);
        }

        public override void TransitionOut()
        {
            base.TransitionOut();
            musicSlider.OnValueChanged -= OnMusicVolumeChanged;
            vfxSlider.OnValueChanged -= OnVfxVolumeChanged;
        }

        private void OnMusicVolumeChanged(float volume)
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnMusicVolumeSliderUpdated, volume);
        }

        private void OnVfxVolumeChanged(float volume)
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnVfxVolumeSliderUpdated, volume);
        }
    }
}

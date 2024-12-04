namespace PlanesRemake.Runtime.UI.Views
{
    using System;

    using UnityEngine;
    
    using PlanesRemake.Runtime.UI.CoreElements;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Localization;
    using PlanesRemake.Runtime.Sound;
    using PlanesRemake.Runtime.UI.Views.DataContainers;
    using UnityEngine.EventSystems;

    public class OptionsMenuView : BaseView
    {
        [Serializable]
        private struct LanguageButtonPair
        {
            public BaseButton languageButton;
            public SystemLanguage languageAssociated;
        }

        [SerializeField]
        private BaseSlider musicSlider = null;

        [SerializeField]
        private BaseSlider vfxSlider = null;

        [SerializeField]
        private LanguageButtonPair[] languageButtonPairs = new LanguageButtonPair[0];

        private LocalizationManager localizationManager = null;

        #region Unity Region

        protected override void Awake()
        {
            base.Awake();
        }

        #endregion

        public override void Initialize(Camera uiCamera, AudioManager sourceAudioManager, ViewInjectableData viewInjectableData, LocalizationManager sourceLocalizationManager, EventSystem eventSystem)
        {
            base.Initialize(uiCamera, sourceAudioManager, viewInjectableData, sourceLocalizationManager, eventSystem);
            localizationManager = sourceLocalizationManager;
        }

        public override void TransitionIn(int sourceInteractableGroupId)
        {
            base.TransitionIn(sourceInteractableGroupId);
            musicSlider.OnValueChanged += OnMusicVolumeChanged;
            vfxSlider.OnValueChanged += OnVfxVolumeChanged;
            musicSlider.UpdateSliderValue(audioManager.CurrentMusicVolume);
            vfxSlider.UpdateSliderValue(audioManager.CurrentVfxVolume);
            
            foreach (LanguageButtonPair languageButtonPair in languageButtonPairs)
            {
                languageButtonPair.languageButton.onButtonPressed += () =>
                    EventDispatcher.Instance.Dispatch(UiEvents.OnLanguageButtonPressed, languageButtonPair.languageAssociated);
                languageButtonPair.languageButton.onButtonPressed += UpdateLanguageButtonSelected;
                languageButtonPair.languageButton.SetInteractable(languageButtonPair.languageAssociated != localizationManager.LanguageSelected);
            }
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

        private void UpdateLanguageButtonSelected()
        {
            foreach (LanguageButtonPair languageButtonPair in languageButtonPairs)
            {
                languageButtonPair.languageButton.SetInteractable(languageButtonPair.languageAssociated != localizationManager.LanguageSelected);
            }
        }
    }
}

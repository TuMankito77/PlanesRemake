namespace PlanesRemake.Runtime.UI.Views
{
    using PlanesRemake.Runtime.Localization;
    using PlanesRemake.Runtime.Sound;
    using PlanesRemake.Runtime.UI.Views.DataContainers;
    using UnityEngine;
    using UnityEngine.UI;

    public class MessageWindowView : BaseView
    {
        [SerializeField]
        private Text messageTextComponent = null;

        public override void Initialize(Camera uiCamera, AudioManager sourceAudioManager, ViewInjectableData viewInjectableData, LocalizationManager localizationManager)
        {
            base.Initialize(uiCamera, sourceAudioManager, viewInjectableData, localizationManager);
            MessageViewData messageViewData = viewInjectableData as MessageViewData;

            if(messageViewData != null)
            {
                messageTextComponent.text = messageViewData.Message;
            }
        }
    }
}


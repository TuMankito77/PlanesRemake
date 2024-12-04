namespace PlanesRemake.Runtime.UI.Views
{
    using System;
    
    using UnityEngine;
    using UnityEngine.UI;

    using PlanesRemake.Runtime.UI.CoreElements;
    using PlanesRemake.Runtime.Gameplay;
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Utils;
    using PlanesRemake.Runtime.Gameplay.StorableClasses;
    using PlanesRemake.Runtime.Sound;
    using PlanesRemake.Runtime.UI.Views.DataContainers;
    using PlanesRemake.Runtime.Localization;
    using UnityEngine.EventSystems;

    public class ShopView : BaseView, IListener
    {
        [SerializeField]
        private BaseButton purchaseButton = null;

        [SerializeField]
        private BaseButton selectButton = null;

        [SerializeField]
        private BaseButton leftArrowButton = null;

        [SerializeField]
        private BaseButton rightArrowButton = null;

        [SerializeField]
        private Text coinsText = null;

        [SerializeField]
        private Text priceText = null;

        [SerializeField]
        private Text ownedText = null;

        [SerializeField]
        private AircraftDatabase aircraftDatabase = null;

        private int aircraftDisplayedIndex = 0;
        private PlayerInformation playerInformation = null;
        private LocalizationManager localizationManager = null;

        public override void TransitionIn(int sourceInteractableGroupId)
        {
            base.TransitionIn(sourceInteractableGroupId);
            purchaseButton.onButtonPressed += OnPurchaseAircraftButtonPressed;
            selectButton.onButtonPressed += OnSelectAircraftButtonPressed;
            leftArrowButton.onButtonPressed += OnLeftArrowButtonPressed;
            rightArrowButton.onButtonPressed += OnRightArrowButtonPressed;
            EventDispatcher.Instance.AddListener(this, typeof(UiEvents));
        }

        public override void TransitionOut()
        {
            base.TransitionOut();
            purchaseButton.onButtonPressed -= OnPurchaseAircraftButtonPressed;
            selectButton.onButtonPressed -= OnSelectAircraftButtonPressed;
            leftArrowButton.onButtonPressed -= OnLeftArrowButtonPressed;
            rightArrowButton.onButtonPressed -= OnRightArrowButtonPressed;
            EventDispatcher.Instance.RemoveListener(this, typeof(UiEvents));
            EventDispatcher.Instance.Dispatch(UiEvents.OnSetShowcaseAircraft, playerInformation.AircraftSelected);
        }

        private void OnRightArrowButtonPressed()
        {
            ShowNextAircraftInfo(turnRight: true);
        }

        private void OnLeftArrowButtonPressed()
        {
            ShowNextAircraftInfo(turnRight: false);
        }

        private void OnSelectAircraftButtonPressed()
        {
            string aircraftId = aircraftDatabase.Ids[aircraftDisplayedIndex];
            EventDispatcher.Instance.Dispatch(UiEvents.OnSelectAircraftButtonPressed, aircraftId);
        }

        private void OnPurchaseAircraftButtonPressed()
        {
            string aircraftId = aircraftDatabase.Ids[aircraftDisplayedIndex];
            int price =  aircraftDatabase.GetFile(aircraftId).Price;
            Tuple<string, int> aircraftIdPriceTuple = new Tuple<string, int>(aircraftId, price);
            EventDispatcher.Instance.Dispatch(UiEvents.OnPurchaseAircraftButtonPressed, aircraftIdPriceTuple);
        }

        private void ShowNextAircraftInfo(bool turnRight)
        {
            aircraftDisplayedIndex = turnRight ? 
                MathfExtensions.Mod(aircraftDisplayedIndex + 1, aircraftDatabase.Ids.Count) :
                MathfExtensions.Mod(aircraftDisplayedIndex - 1, aircraftDatabase.Ids.Count);

            string aircraftId = aircraftDatabase.Ids[aircraftDisplayedIndex];
            EventDispatcher.Instance.Dispatch(UiEvents.OnSetShowcaseAircraft, aircraftId);
            
            if (playerInformation.AircraftsPurchased.Contains(aircraftId))
            {
                ShowAircraftOwnedConfiguration();
            }
            else
            {
                ShowPurchasableAircraftConfiguration(aircraftDatabase.GetFile(aircraftId).Price);
            }
        }

        private void ShowAircraftOwnedConfiguration()
        {
            purchaseButton.SetInteractable(false);
            priceText.transform.parent.gameObject.SetActive(false);
            selectButton.SetInteractable(true);
            ownedText.gameObject.SetActive(true);
        }

        private void ShowPurchasableAircraftConfiguration(int price)
        {
            purchaseButton.SetInteractable(true);
            priceText.transform.parent.gameObject.SetActive(true);
            string priceTextLocalized = localizationManager.GetLocalizedText("Text.Price");
            priceText.text = string.Format(priceTextLocalized, price);
            selectButton.SetInteractable(false);
            ownedText.gameObject.SetActive(false);
        }

        #region UiEvents

        public void HandleEvent(IComparable eventName, object data)
        {
            switch(eventName)
            {
                case UiEvents uiEvent:
                    {
                        HandleUiEvents(uiEvent, data);
                        break;
                    }

                default:
                    {
                        LoggerUtil.LogError($"{GetType()} - The event {eventName} is not handled by this class. You may need to unsubscribe.");
                        break;
                    }
            }
        }

        #endregion

        public override void Initialize(Camera uiCamera, AudioManager sourceAudioManager, ViewInjectableData viewInjectableData, LocalizationManager sourceLocalizationManager, EventSystem eventSystem)
        {
            base.Initialize(uiCamera, sourceAudioManager, viewInjectableData, sourceLocalizationManager, eventSystem);
            localizationManager = sourceLocalizationManager;
            ShopViewData shopViewData = viewInjectableData as ShopViewData;
            
            if(shopViewData != null)
            {
                playerInformation = shopViewData.PlayerInformation;
                string coinsTextLocalized = localizationManager.GetLocalizedText("Text.Coins");
                coinsText.text = string.Format(coinsTextLocalized, playerInformation.CoinsCollected);

                for (int i = 0; i < aircraftDatabase.Ids.Count; i++)
                {
                    if (playerInformation.AircraftSelected == aircraftDatabase.Ids[i])
                    {
                        aircraftDisplayedIndex = i;
                        break;
                    }
                }

                ShowAircraftOwnedConfiguration();
            }
        }

        private void HandleUiEvents(UiEvents uiEvent, object data)
        {
            switch(uiEvent)
            {
                case UiEvents.OnUpdatePlayerInformation:
                    {
                        playerInformation = data as PlayerInformation;
                        string coinsTextLocalized = localizationManager.GetLocalizedText("Text.Coins");
                        coinsText.text = string.Format(coinsTextLocalized, playerInformation.CoinsCollected);
                        string aircraftId = aircraftDatabase.Ids[aircraftDisplayedIndex];

                        if (playerInformation.AircraftsPurchased.Contains(aircraftId))
                        {
                            ShowAircraftOwnedConfiguration();
                        }
                        else
                        {
                            ShowPurchasableAircraftConfiguration(aircraftDatabase.GetFile(aircraftId).Price);
                        }

                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
    }
}


namespace PlanesRemake.Runtime.UI
{
    using System;

    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class BaseButton : MonoBehaviour
    {
        public event Action onButtonPressed = null;

        private Button buttonComponent = null;

        #region Unity Methods

        private void Awake()
        {
            buttonComponent = GetComponent<Button>();
            buttonComponent.onClick.AddListener(OnButtonPressed);
        }

        #endregion

        private void OnButtonPressed()
        {
            onButtonPressed?.Invoke();
        }
    }
}

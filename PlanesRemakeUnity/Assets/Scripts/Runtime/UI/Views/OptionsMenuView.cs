namespace PlanesRemake.Runtime.UI.Views
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.UI.CoreElements;

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
    }
}

namespace PlanesRemastered.Runtime.Core
{
    using System.Collections.Generic;

    using UnityEngine;
    
    using PlanesRemastered.Runtime.UI.Views;

    [CreateAssetMenu(fileName = "ViewsContainer", menuName = "Ui/ViewsContainer")]
    public class ViewsContainer : ScriptableObject
    {
        [SerializeField]
        private LayerMask viewsLayerMask = default(LayerMask);
        
        [SerializeField]
        private BaseView[] views = new BaseView[0];

        //Create a custom inspector to add the views by id
        private Dictionary<string, BaseView> viewsById = new Dictionary<string, BaseView>();

        public IReadOnlyDictionary<string, BaseView> ViewsById => viewsById;
        public LayerMask ViewsLayerMask => viewsLayerMask;

        public void Initialize()
        {
            if(views.Length <= 0)
            {
                return;
            }

            viewsById = new Dictionary<string, BaseView>()
            {
                { ViewIds.MainMenu, views[0] },
                { ViewIds.Hud, views[1] },
                { ViewIds.PauseMenu, views[2] }
            };
        }
    }
}
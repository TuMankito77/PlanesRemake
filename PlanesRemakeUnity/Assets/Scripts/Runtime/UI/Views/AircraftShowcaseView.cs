namespace PlanesRemake.Runtime.UI.Views
{
    using UnityEngine;

    public class AircraftShowcaseView : BaseView
    {
        [SerializeField]
        private GameObject aircraftStudioShowcasePrefab = null;

        private GameObject aircraftStudioShowcase = null;

        public override void TransitionIn()
        {
            base.TransitionIn();
            aircraftStudioShowcase = Instantiate(aircraftStudioShowcasePrefab);
            DontDestroyOnLoad(aircraftStudioShowcase);
        }

        public override void TransitionOut()
        {
            base.TransitionOut();
            Destroy(aircraftStudioShowcase);
        }
    }
}


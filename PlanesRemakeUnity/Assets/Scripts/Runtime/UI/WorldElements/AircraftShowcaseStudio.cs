namespace PlanesRemake.Runtime.UI.WorldElements
{
    using UnityEngine;

    public class AircraftShowcaseStudio : MonoBehaviour
    {
        [SerializeField]
        private Transform aircraftPlacement = null;

        private GameObject aircraftDisplayed = null;

        public void UpdateAircraftDisplayed(GameObject aircraftPrefab)
        {
            if(aircraftPlacement != null)
            {
                Destroy(aircraftDisplayed);
            }

            aircraftDisplayed = Instantiate(aircraftPrefab, aircraftPlacement);
        }
    }
}

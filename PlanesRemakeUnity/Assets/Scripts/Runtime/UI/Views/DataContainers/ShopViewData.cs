namespace PlanesRemake.Runtime.UI.Views.DataContainers
{
    using PlanesRemake.Runtime.Gameplay.StorableClasses;
    
    public class ShopViewData : ViewInjectableData
    {
        public PlayerInformation PlayerInformation { get; private set; } = null;

        public ShopViewData(PlayerInformation playerInformation)
        {
            PlayerInformation = playerInformation;
        }
    }
}


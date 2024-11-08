namespace PlanesRemake.Runtime.Gameplay
{
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Sound;

    public class Fuel : BasePickUpItem
    {
        protected override GameplayEvents GameplayEventToDispatch => GameplayEvents.OnFuelCollected;
        protected override string PickUpClipId => ClipIds.FUEL_CLIP;
    }
}

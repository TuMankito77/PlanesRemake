namespace PlanesRemake.Runtime.Gameplay
{
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Sound;

    public class Magnet : BasePickUpItem
    {
        protected override GameplayEvents GameplayEventToDispatch => GameplayEvents.OnMagnetCollected;
        protected override string PickUpClipId => ClipIds.MAGNET_CLIP;
    }
}


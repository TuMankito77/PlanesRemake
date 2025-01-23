namespace PlanesRemake.Runtime.Gameplay.PickUps
{
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Sound;

    public class Shield : BasePickUpItem
    {
        protected override GameplayEvents GameplayEventToDispatch => GameplayEvents.OnShieldCollected;
        protected override string PickUpClipId => ClipIds.SHIELD_CLIP;
    }
}

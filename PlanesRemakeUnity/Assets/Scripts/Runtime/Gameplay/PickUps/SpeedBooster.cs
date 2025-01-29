namespace PlanesRemake.Runtime.Gameplay.PickUps
{
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Sound;

    public class SpeedBooster : BasePickUpItem
    {
        protected override GameplayEvents GameplayEventToDispatch => GameplayEvents.OnSpeedBoosterCollected;
        protected override string PickUpClipId => ClipIds.SPEED_BOOSTER_CLIP;
    }
}


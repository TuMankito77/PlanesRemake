namespace PlanesRemake.Runtime.Gameplay.PickUps
{
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Sound;

    public class CoinMagnet : BasePickUpItem
    {
        protected override GameplayEvents GameplayEventToDispatch => GameplayEvents.OnCoinMagnetCollected;
        protected override string PickUpClipId => ClipIds.MAGNET_CLIP;
    }
}


namespace PlanesRemake.Runtime.Gameplay.PickUps
{
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Sound;

    public class Coin : BasePickUpItem
    {
        protected override GameplayEvents GameplayEventToDispatch => GameplayEvents.OnCoinCollected;
        protected override string PickUpClipId => ClipIds.COIN_CLIP;
    }
}

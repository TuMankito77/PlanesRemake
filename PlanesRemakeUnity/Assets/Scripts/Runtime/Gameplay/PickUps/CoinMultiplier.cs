namespace PlanesRemake.Runtime.Gameplay.PickUps
{
    using PlanesRemake.Runtime.Events;
    using PlanesRemake.Runtime.Sound;

    public class CoinMultiplier : BasePickUpItem
    {
        protected override GameplayEvents GameplayEventToDispatch => GameplayEvents.OnCoinMultiplierCollected;
        protected override string PickUpClipId => ClipIds.COIN_MULTIPLIER_CLIP;
    }
}

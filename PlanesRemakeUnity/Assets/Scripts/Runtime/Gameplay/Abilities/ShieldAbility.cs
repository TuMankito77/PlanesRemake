namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using UnityEngine;

    public class ShieldAbility : VisualAbility
    {
        private Collider crashDetectionCollider = null;

        protected override bool IsAbilityTimerTickEnabled => true;

        public ShieldAbility(GameObject sourceOwner, ShieldAbilityData shieldAbilityData, Collider sourceCrashDetectionCollider) : 
            base(sourceOwner, shieldAbilityData)
        {
            crashDetectionCollider = sourceCrashDetectionCollider;
        }

        public override void Activate()
        {
            base.Activate();
            crashDetectionCollider.enabled = false;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            crashDetectionCollider.enabled = true;
        }
    }
}

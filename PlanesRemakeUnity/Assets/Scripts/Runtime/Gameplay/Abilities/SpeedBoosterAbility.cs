namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using UnityEngine;

    public class SpeedBoosterAbility : VisualAbility
    {
        private Aircraft aircraft = null;
        private int speedMultiplier = 1;

        public SpeedBoosterAbility(GameObject sourceOwner, AbilityData sourceAbilityData, Aircraft sourceAircraft) : 
            base(sourceOwner, sourceAbilityData)
        {
            aircraft = sourceAircraft;
            SpeedBoosterAbilityData speedBoosterAbilityData = sourceAbilityData as SpeedBoosterAbilityData;
            speedMultiplier = speedBoosterAbilityData.SpeedMultiplier;
        }

        public override void Activate()
        {
            base.Activate();
            aircraft.UpdateMovementSpeed(aircraft.MovementSpeed * speedMultiplier);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            aircraft.UpdateMovementSpeed(aircraft.MovementSpeed / speedMultiplier);
        }
    }
}


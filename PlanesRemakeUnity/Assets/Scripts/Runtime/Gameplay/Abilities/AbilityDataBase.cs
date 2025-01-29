namespace PlanesRemake.Runtime.Gameplay.Abilities
{
    using UnityEngine;

    [CreateAssetMenu(fileName = ABILITY_DATABASE_ASSET_NAME, menuName = "Database/AbilitiesDatabase")]
    public class AbilityDataBase : ScriptableObject
    {
        private const string ABILITY_DATABASE_ASSET_NAME = "AbilitiesDatabase";
        public const string ABILITY_DATABASE_SCRIPTABLE_OBJECT_PATH = "MainLevel/Abilities/AbilitiesDatabase";

        [SerializeField]
        private MagnetAbilityData coinMagnetAbilityData = null;

        [SerializeField]
        private ShieldAbilityData shieldAbilityData = null;

        [SerializeField]
        private SpeedBoosterAbilityData speedBoosterAbilityData = null;

        public MagnetAbilityData CoinMagnetAbilityData => coinMagnetAbilityData;
        public ShieldAbilityData ShieldAbilityData => shieldAbilityData;
        public SpeedBoosterAbilityData SpeedBoosterAbilityData => speedBoosterAbilityData;
    }
}

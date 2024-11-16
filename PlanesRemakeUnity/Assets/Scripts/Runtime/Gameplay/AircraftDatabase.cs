namespace PlanesRemake.Runtime.Gameplay
{
    using System;

    using UnityEngine;
    
    using PlanesRemake.Runtime.Database;

    [Serializable]
    public class AircraftData
    {
        [SerializeField]
        private Aircraft aircraftPrefab = null;

        [SerializeField]
        private GameObject showcaseAircraftPrefab = null;

        [SerializeField]
        private int price = 100;

        public Aircraft AircraftPrefab => aircraftPrefab;
        public GameObject ShowcaseAircraftPrefab => showcaseAircraftPrefab;
        public int Price => price;
    }

    [CreateAssetMenu(fileName = AIRCRAFTS_DATABASE_ASSET_NAME, menuName = "Database/AircraftsDatabase")]
    public class AircraftDatabase : FileDatabase<AircraftData>
    {
        public const string AIRCRAFTS_DATABASE_SCRIPTABLE_OBJECT_PATH = "MainLevel/AircraftsDatabase";

        private const string AIRCRAFTS_DATABASE_ASSET_NAME = "AircraftsDatabase";
        
        public override string FileDatabasePathScriptableObjectPath => AIRCRAFTS_DATABASE_SCRIPTABLE_OBJECT_PATH;

        protected override string TemplateIdsContainerScriptPath => "Assets/Scripts/Editor/Database/Templates/TemplateAircraftIds.txt";
        protected override string IdsContainerClassScriptPath => "Assets/Scripts/Runtime/Gameplay/AircraftIds.cs";
        protected override string TemplateIdVariableSlot => "#AircraftId#";
        protected override string IdScriptLineStart => "#";
        protected override string IdScriptLineEnd => ";";
    }
}

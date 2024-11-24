namespace PlanesRemake.Runtime.Localization
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Database;

    [CreateAssetMenu(fileName = LOCALIZATION_DATABASE_ASSET_NAME, menuName = "Database/LocalizationDatabase")]
    public class LocalizationDatabase : FileDatabase<TextAsset>
    {
        public const string LOCALIZATION_DATABASE_SCRIPTABLE_OBJECT_PATH = "Localization/LocalizationDatabase";

        private const string LOCALIZATION_DATABASE_ASSET_NAME = "LocalizationDatabase";

        public override string FileDatabasePathScriptableObjectPath => LOCALIZATION_DATABASE_SCRIPTABLE_OBJECT_PATH;

        protected override string TemplateIdsContainerScriptPath => "Assets/Scripts/Editor/Database/Templates/TemplateLanguageIds.txt";
        protected override string IdsContainerClassScriptPath => "Assets/Scripts/Runtime/Localization/LanguageIds.cs";
        protected override string TemplateIdVariableSlot => "#LanguageId#";
        protected override string IdScriptLineStart => "#";
        protected override string IdScriptLineEnd => ";";
    }
}

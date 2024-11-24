namespace PlanesRemake.Editor.Localization
{
    using UnityEngine;
    
    using PlanesRemake.Editor.Database;
    using PlanesRemake.Runtime.Localization;
    
    using UnityEditor;

    [CustomEditor(typeof(LocalizationDatabase))]
    public class LocalizationDatabaseInspector : GenerateDatabaseButtonInspector<LocalizationDatabase, TextAsset>
    {
    
    }
}


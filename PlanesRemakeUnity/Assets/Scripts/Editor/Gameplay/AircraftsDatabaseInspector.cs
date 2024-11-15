namespace PlanesRemake.Editor.Gameplay
{
    using UnityEditor;

    using PlanesRemake.Editor.Database;
    using PlanesRemake.Runtime.Gameplay;

    [CustomEditor(typeof(AircraftDatabase))]
    public class AircraftsDatabaseInspector : GenerateDatabaseButtonInspector<AircraftDatabase, AircraftData>
    {
    
    }
}


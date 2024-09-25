namespace PlanesRemake.Editor.Sound
{
    using UnityEditor;
    
    using PlanesRemake.Editor.Database;
    using PlanesRemake.Runtime.Sound;
    using UnityEngine;

    [CustomEditor(typeof(ClipsDatabase))]
    public class ClipsDatabaseInspector : GenerateDatabaseButtonInspector<ClipsDatabase, AudioClip>
    {
        
    }
}


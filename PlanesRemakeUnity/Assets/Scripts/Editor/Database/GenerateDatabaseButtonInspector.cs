namespace PlanesRemake.Editor.Database
{
    using UnityEngine;
    using UnityEditor;

    using PlanesRemake.Runtime.Database;

    public class GenerateDatabaseButtonInspector<T1, T2> : Editor where T1 : FileDatabase<T2> where T2 : class
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            T1 cllipIdsDatabase = target as T1;
            
            if (GUILayout.Button($"Regenerate {cllipIdsDatabase.GetType().Name} ID class file"))
            {
                cllipIdsDatabase.GenerateIdsContainerClassFile();
            }
        }
    }
}


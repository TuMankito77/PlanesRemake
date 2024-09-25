namespace PlanesRemake.Editor.UI
{
    using UnityEditor;

    using PlanesRemake.Runtime.UI.Views;
    using PlanesRemake.Editor.Database;

    [CustomEditor(typeof(ViewsDatabase))]
    public class ViewsDatabaseInspector : GenerateDatabaseButtonInspector<ViewsDatabase, BaseView>
    {
        
    }
}


using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Scratch_Map))]
public class MapInspector : Editor{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        DrawDefaultInspector();

        if (GUILayout.Button("Regenerate"))
        {
            Scratch_Map map = (Scratch_Map)target;
            map.CreateMap();
        }
    }
}

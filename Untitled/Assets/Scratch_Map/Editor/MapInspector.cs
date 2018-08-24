using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Scratch_Map))]
public class MapInspector : Editor
{
    private Scratch_Map map;

    private void OnEnable()
    {
        map = target as Scratch_Map;
    }

    private void OnDisable()
    {
        map = null;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        DrawDefaultInspector();

        if (GUILayout.Button("Clear All"))
        {
            //for (int i = map.transform.childCount - 1; i >= 0; i--)
            //{
            //    GameObject.DestroyImmediate(map.transform.GetChild(i).gameObject);
            //}
            map.ClearOldMeshes();
        }

        if (GUILayout.Button("Regenerate"))
        {
            map.CreateMap();
        }
    }
}

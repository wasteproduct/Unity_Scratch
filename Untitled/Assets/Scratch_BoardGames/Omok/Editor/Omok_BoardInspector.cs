using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Omok_Board))]
public class Omok_BoardInspector : Editor{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        DrawDefaultInspector();

        if (GUILayout.Button("Regenerate"))
        {
            Omok_Board omokBoard = (Omok_Board)target;

            omokBoard.SetBoard();
        }
    }
}

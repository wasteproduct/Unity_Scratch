using UnityEngine;
using TileData;

public class Scratch_Highlighter : MonoBehaviour
{
    public GameObject tileMap;

    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        offset = new Vector3(0.5f, 0.0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100.0f, 1 << LayerMask.NameToLayer("Tile Map")) == true)
        {
            int x = (int)(hitInfo.point.x + offset.x);
            int z = (int)(hitInfo.point.z + offset.z);

            Scratch_TileData currentTile = tileMap.GetComponent<Scratch_Map>().MapData.TileData[x, z];

            this.transform.position = new Vector3((float)currentTile.X, 0.0f, (float)currentTile.Z);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapData;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Scratch_Map : MonoBehaviour
{
    public MapSize mapSize;
    public GameObject dungeonFloor;
    public GameObject dungeonWall;

    // Use this for initialization
    void Start()
    {
        //CreateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Scratch_MapData MapData { get; private set; }

    public void CreateMap()
    {
        MapData = new Scratch_MapData(mapSize);

        SetMeshes();

        CombineMeshes();
    }

    private void CombineMeshes()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh.Clear();

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        //this.GetComponent<MeshRenderer>().material = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;

        CombineInstance[] instances = new CombineInstance[meshFilters.Length];

        Mesh combinedMesh = new Mesh();

    }

    private void SetMeshes()
    {
        for (int z = 0; z < MapData.TilesColumn; z++)
        {
            for (int x = 0; x < MapData.TilesRow; x++)
            {
                switch (MapData.TileData[x, z].Type)
                {
                    case TileData.TileType.None:
                        break;
                    case TileData.TileType.Floor:
                        Instantiate<GameObject>(dungeonFloor, new Vector3((float)x, 0.0f, (float)z), Quaternion.identity, this.transform);
                        break;
                    case TileData.TileType.Wall:
                        BuildWall(x, z);
                        break;
                }
            }
        }
    }

    private void BuildWall(int x, int z)
    {
        for (int i = 0; i < MapData.TileData[x, z].Direction.Count; i++)
        {
            Quaternion wallDirection = Quaternion.identity;

            switch (MapData.TileData[x, z].Direction[i])
            {
                case TileData.WallDirection.Left:
                    wallDirection = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                    break;
                case TileData.WallDirection.Right:
                    wallDirection = Quaternion.Euler(0.0f, -90.0f, 0.0f);
                    break;
                case TileData.WallDirection.There:
                    wallDirection = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    break;
            }

            Instantiate<GameObject>(dungeonWall, new Vector3((float)x + .5f, 0.0f, (float)z + .5f), wallDirection, this.transform);
        }
    }
}

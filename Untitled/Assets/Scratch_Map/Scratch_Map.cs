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
    public enum MapSize
    {
        Small,
        Medium,
        Large
    }

    public MapSize mapSize;
    public GameObject dungeonFloor;

    [HideInInspector]
    public int tilesRow, tilesColumn, roomsNumber;

    // Use this for initialization
    void Start()
    {
        switch (mapSize)
        {
            case MapSize.Small:
                tilesRow = tilesColumn = 64;
                break;
            case MapSize.Medium:
                tilesRow = tilesColumn = 128;
                break;
            case MapSize.Large:
                break;
        }

        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateMap()
    {
        MapData = new Scratch_MapData(tilesRow, tilesColumn);

        SetMeshes();
    }

    private void SetMeshes()
    {
        for (int z = 0; z < tilesColumn; z++)
        {
            for (int x = 0; x < tilesRow; x++)
            {
                switch (MapData.TileData[x, z].Type)
                {
                    case TileData.TileType.None:
                        break;
                    case TileData.TileType.Floor:
                        Instantiate<GameObject>(dungeonFloor, new Vector3((float)x, 0.0f, (float)z), Quaternion.identity, this.transform);
                        break;
                    case TileData.TileType.Wall:
                        break;
                }
            }
        }
    }

    public Scratch_MapData MapData { get; private set; }
}

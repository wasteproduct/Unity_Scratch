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
    public GameObject debugTile;
    public GameObject dungeonFloor;
    public GameObject dungeonWall;
    public GameObject dungeonDoor;

    // Use this for initialization
    void Start()
    {
        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Scratch_MapData MapData { get; private set; }

    public void CreateMap()
    {
        MapData = new Scratch_MapData(mapSize);

        ClearAll();

        SetMeshes();

        CombineMeshes();

        SetObjects();
    }

    public void ClearAll()
    {
        //Destroy(this.GetComponent<MeshFilter>().mesh);
        DestroyImmediate(this.GetComponent<MeshFilter>().sharedMesh);
        this.GetComponent<MeshFilter>().sharedMesh = null;

        for (int i = this.transform.childCount - 1; i >= 0; i--)
        {
            //Destroy(this.transform.GetChild(i).gameObject);
            DestroyImmediate(this.transform.GetChild(i).gameObject);
        }
    }

    private void SetObjects()
    {
        for (int i = 0; i < MapData.Doors.Count; i++)
        {
            Quaternion wallDirection = Quaternion.identity;

            switch (MapData.Doors[i].Direction[0])
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

            Instantiate<GameObject>(dungeonDoor, new Vector3((float)MapData.Doors[i].X + .5f, 0.0f, (float)MapData.Doors[i].Z + .5f), wallDirection, this.transform);
            Instantiate<GameObject>(debugTile, new Vector3((float)MapData.Doors[i].X, 0.0f, (float)MapData.Doors[i].Z), Quaternion.identity, this.transform);
        }

        for(int z=0;z<MapData.TilesColumn;z++)
        {
            for(int x=0;x<MapData.TilesRow;x++)
            {
                if(MapData.TileData[x,z].Type==TileData.TileType.DoorWall)
                {
                    Instantiate<GameObject>(debugTile, new Vector3((float)x, 0.0f, (float)z), Quaternion.identity, this.transform);
                }
            }
        }
    }

    private void CombineMeshes()
    {
        MeshFilter[] meshFilters = this.GetComponentsInChildren<MeshFilter>(true);

        CombineInstance[] instances = new CombineInstance[meshFilters.Length];

        List<CombineInstance> floors = new List<CombineInstance>();
        List<CombineInstance> walls = new List<CombineInstance>();
        List<Material> materials = new List<Material>();

        Material floorMaterial = null;
        Material wallMaterial = null;

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].transform == this.transform) continue;

            instances[i] = new CombineInstance();

            instances[i].mesh = meshFilters[i].sharedMesh;
            instances[i].transform = meshFilters[i].transform.localToWorldMatrix;

            if (meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial.name == "floor_A")
            {
                floorMaterial = meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial;
                floors.Add(instances[i]);
            }
            else if (meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial.name == "wall_A")
            {
                wallMaterial = meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial;
                walls.Add(instances[i]);
            }
        }

        materials.Add(floorMaterial);
        materials.Add(wallMaterial);

        Mesh combinedFloor = new Mesh();
        combinedFloor.CombineMeshes(floors.ToArray());

        Mesh combinedWall = new Mesh();
        combinedWall.CombineMeshes(walls.ToArray());

        CombineInstance[] combinedInstances = new CombineInstance[2];
        combinedInstances[0].mesh = combinedFloor;
        combinedInstances[0].transform = this.transform.localToWorldMatrix;
        combinedInstances[1].mesh = combinedWall;
        combinedInstances[1].transform = this.transform.localToWorldMatrix;

        Mesh ultimateMesh = new Mesh();
        ultimateMesh.CombineMeshes(combinedInstances, false);

        this.GetComponent<MeshRenderer>().sharedMaterials = new Material[2];

        for (int i = 0; i < 2; i++)
        {
            this.GetComponent<MeshRenderer>().sharedMaterials = materials.ToArray();
        }

        this.GetComponent<MeshFilter>().mesh = ultimateMesh;

        for (int i = this.transform.childCount - 1; i >= 0; i--)
        {
            //Destroy(this.transform.GetChild(i).gameObject);
            DestroyImmediate(this.transform.GetChild(i).gameObject);
        }
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
                    case TileData.TileType.Door:
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

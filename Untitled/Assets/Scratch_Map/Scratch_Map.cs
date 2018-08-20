using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Scratch_Map : MonoBehaviour {
    public int tilesRow;
    public int tilesColumn;

    [HideInInspector]
    public Scratch_TileData[,] tilesData;

	// Use this for initialization
	void Start () {
        CreateMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateMap()
    {
        tilesData = new Scratch_TileData[tilesRow, tilesColumn];
    }
}

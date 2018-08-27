using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scratch_Player : MonoBehaviour
{
    public GameObject tileMap;

    // Use this for initialization
    void Start()
    {
        float startingX = (float)tileMap.GetComponent<Scratch_Map>().MapData.StartingTile.X;
        float startingZ = (float)tileMap.GetComponent<Scratch_Map>().MapData.StartingTile.Z;

        this.transform.position = new Vector3(startingX, 0.0f, startingZ);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

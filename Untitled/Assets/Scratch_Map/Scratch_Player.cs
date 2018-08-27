using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStar;

public class Scratch_Player : MonoBehaviour
{
    public GameObject tileMap;

    private Scratch_AStar aStar;

    // Use this for initialization
    void Start()
    {
        float startingX = (float)tileMap.GetComponent<Scratch_Map>().MapData.StartingTile.X;
        float startingZ = (float)tileMap.GetComponent<Scratch_Map>().MapData.StartingTile.Z;

        this.transform.position = new Vector3(startingX, 0.0f, startingZ);

        aStar = new Scratch_AStar(tileMap.GetComponent<Scratch_Map>().MapData);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {

        }
    }
}

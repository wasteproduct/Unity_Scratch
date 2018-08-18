using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omok_Stone : MonoBehaviour {
    public GameObject stoneBody;
    public Material blackMaterial;
    public Material whiteMaterial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMaterial(Omok_BoardTile.TileOccupation currentTurn)
    {
        switch (currentTurn)
        {
            case Omok_BoardTile.TileOccupation.Black:
                stoneBody.GetComponent<MeshRenderer>().material = blackMaterial;
                break;
            case Omok_BoardTile.TileOccupation.White:
                stoneBody.GetComponent<MeshRenderer>().material = whiteMaterial;
                break;
        }
    }
}

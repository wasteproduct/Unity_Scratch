using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Omok : MonoBehaviour {
    public GameObject board;
    public GameObject stone;

    private Omok_Board omokBoard;
    private Omok_BoardTile.TileOccupation currentTurn;
    private bool gameOver;

	// Use this for initialization
	void Start () {
        omokBoard = board.GetComponent<Omok_Board>();
        currentTurn = Omok_BoardTile.TileOccupation.Black;
        gameOver = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (gameOver == true) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo))
            {
                int x = (int)hitInfo.point.x;
                int z = (int)hitInfo.point.z;

                if (omokBoard.boardData[x, z].Occupier != Omok_BoardTile.TileOccupation.None) return;

                GameObject newStone = Instantiate(stone, new Vector3((float)x, 0.0f, (float)z), Quaternion.identity);
                newStone.GetComponent<Omok_Stone>().SetMaterial(currentTurn);

                omokBoard.boardData[x, z].Occupier = currentTurn;

                if (CheckCompletion(x, z) == true)
                {
                    gameOver = true;
                    Debug.Log(currentTurn + " wins.");
                    return;
                }

                currentTurn++;
                if (currentTurn > Omok_BoardTile.TileOccupation.White) currentTurn = Omok_BoardTile.TileOccupation.Black;
            }
        }
    }

    private bool CheckCompletion(int x, int z)
    {
        if (CheckRowCompletion(x, z) == true) return true;
        if (CheckColumnCompletion(x, z) == true) return true;
        if (CheckDiagonalCompletion(x, z) == true) return true;

        return false;
    }

    private bool CheckDiagonalCompletion(int x, int z)
    {
        int leftMost = x - 4;
        int bottom = z - 4;

        return false;
    }

    private bool CheckColumnCompletion(int x, int z)
    {
        int bottom = z - 4;

        for (int i = bottom; i <= z; i++)
        {
            if (CheckZAvailable(i) == false) continue;

            if ((omokBoard.boardData[x, i].Occupier == currentTurn) && (omokBoard.boardData[x, i + 1].Occupier == currentTurn) &&
                (omokBoard.boardData[x, i + 2].Occupier == currentTurn) && (omokBoard.boardData[x, i + 3].Occupier == currentTurn) &&
                (omokBoard.boardData[x, i + 4].Occupier == currentTurn)) return true;
        }

        return false;
    }

    private bool CheckRowCompletion(int x, int z)
    {
        int leftMost = x - 4;

        for (int i = leftMost; i <= x; i++)
        {
            if (CheckXAvailable(i) == false) continue;

            if ((omokBoard.boardData[i, z].Occupier == currentTurn) && (omokBoard.boardData[i + 1, z].Occupier == currentTurn) &&
                (omokBoard.boardData[i + 2, z].Occupier == currentTurn) && (omokBoard.boardData[i + 3, z].Occupier == currentTurn) &&
                (omokBoard.boardData[i + 4, z].Occupier == currentTurn)) return true;
        }

        return false;
    }

    private bool CheckZAvailable(int z)
    {
        if ((z < 0) || (z >= omokBoard.tilesColumn)) return false;

        return true;
    }

    private bool CheckXAvailable(int x)
    {
        if ((x < 0) || (x >= omokBoard.tilesRow)) return false;

        return true;
    }

    private bool CheckIndexAvailable(int x, int z)
    {
        if ((x < 0) || (x >= omokBoard.tilesRow) || (z < 0) || (z >= omokBoard.tilesColumn)) return false;

        return true;
    }
}

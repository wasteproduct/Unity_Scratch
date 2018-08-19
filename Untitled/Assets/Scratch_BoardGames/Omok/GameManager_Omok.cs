using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Omok : MonoBehaviour {
    public GameObject board;
    public GameObject stone;

    private Omok_Board omokBoard;
    private Omok_BoardTile.TileOccupation currentTurn;
    private int linkCount;
    private bool gameOver;
    private readonly int invalidIndex = -1;

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

                    if (linkCount > 5) TurnOver();

                    Debug.Log(currentTurn + " wins.");

                    return;
                }

                TurnOver();
            }
        }
    }

    private void TurnOver()
    {
        currentTurn++;
        if (currentTurn > Omok_BoardTile.TileOccupation.White) currentTurn = Omok_BoardTile.TileOccupation.Black;
    }

    private bool CheckCompletion(int x, int z)
    {
        if (CheckRowCompletion(z) == true) return true;
        if (CheckColumnCompletion(x) == true) return true;
        if (CheckDiagonalCompletion(x, z) == true) return true;

        return false;
    }

    private bool CheckDiagonalCompletion(int x, int z)
    {
        if (CheckDirectProportional(x, z) == true) return true;
        if (CheckInverseProportional(x, z) == true) return true;

        return false;
    }

    private bool CheckDirectProportional(int x, int z)
    {
        linkCount = 0;

        int xIndex = x;
        int zIndex = z;
        int row = omokBoard.tilesRow;
        int column = omokBoard.tilesColumn;

        //인덱스 초기화
        while(true)
        {
            if((xIndex<=0)||(zIndex<=0))
        }

        while (true)
        {
            int failureCount = 0;

            //검사 시작할 인덱스 색출
            while (true)
            {
                //찾았으면 ++, 검사 시작
                if (omokBoard.boardData[x, index].Occupier == currentTurn)
                {
                    linkCount++;
                    failureCount = 0;
                    break;
                }

                //못 찾았으면 다음 걸 보고
                index++;

                //끝까지 못 찾으면 검사 종료
                if (index >= column) break;

                failureCount++;
                if (failureCount >= column) return false;
            }

            while (true)
            {
                //다음 걸 보고
                index++;

                //다 봤으면 검사 종료
                if (index >= column) break;

                //돌 있으면 ++
                if (omokBoard.boardData[x, index].Occupier == currentTurn) linkCount++;
                //끊겨 있으면
                else
                {
                    //끊긴 시점에서 연결된 돌 수 확인
                    if (linkCount >= 5) return true;

                    //안 끝났으면 카운트 0, 검사 재개
                    linkCount = 0;
                    break;
                }
            }

            if (index >= column) break;
        }

        if (linkCount >= 5) return true;

        return false;
    }

    private bool CheckColumnCompletion(int x)
    {
        linkCount = 0;

        int index = 0;
        int column = omokBoard.tilesColumn;

        while (true)
        {
            int failureCount = 0;

            //검사 시작할 인덱스 색출
            while (true)
            {
                //찾았으면 ++, 검사 시작
                if (omokBoard.boardData[x, index].Occupier == currentTurn)
                {
                    linkCount++;
                    failureCount = 0;
                    break;
                }

                //못 찾았으면 다음 걸 보고
                index++;

                //끝까지 못 찾으면 검사 종료
                if (index >= column) break;

                failureCount++;
                if (failureCount >= column) return false;
            }

            while (true)
            {
                //다음 걸 보고
                index++;

                //다 봤으면 검사 종료
                if (index >= column) break;

                //돌 있으면 ++
                if (omokBoard.boardData[x, index].Occupier == currentTurn) linkCount++;
                //끊겨 있으면
                else
                {
                    //끊긴 시점에서 연결된 돌 수 확인
                    if (linkCount >= 5) return true;

                    //안 끝났으면 카운트 0, 검사 재개
                    linkCount = 0;
                    break;
                }
            }

            if (index >= column) break;
        }

        if (linkCount >= 5) return true;

        return false;
    }

    private bool CheckRowCompletion(int z)
    {
        linkCount = 0;

        int index = 0;
        int row = omokBoard.tilesRow;

        while (true)
        {
            int failureCount = 0;

            //검사 시작할 인덱스 색출
            while(true)
            {
                //찾았으면 ++, 검사 시작
                if (omokBoard.boardData[index, z].Occupier == currentTurn)
                {
                    linkCount++;
                    failureCount = 0;
                    break;
                }

                //못 찾았으면 다음 걸 보고
                index++;

                //끝까지 못 찾으면 검사 종료
                if (index >= row) break;

                failureCount++;
                if (failureCount >= row) return false;
            }

            while (true)
            {
                //다음 걸 보고
                index++;

                //다 봤으면 검사 종료
                if (index >= row) break;

                //돌 있으면 ++
                if (omokBoard.boardData[index, z].Occupier == currentTurn) linkCount++;
                //끊겨 있으면
                else
                {
                    //끊긴 시점에서 연결된 돌 수 확인
                    if (linkCount >= 5) return true;

                    //안 끝났으면 카운트 0, 검사 재개
                    linkCount = 0;
                    break;
                }
            }

            if (index >= row) break;
        }

        if (linkCount >= 5) return true;

        return false;
    }
}

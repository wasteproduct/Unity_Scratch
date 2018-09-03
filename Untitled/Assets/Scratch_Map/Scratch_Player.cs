using System.Collections;
using UnityEngine;
using AStar;
using MapData;
using TileData;

namespace Player
{
    public class Scratch_Player : MonoBehaviour
    {
        public GameObject tileMap;
        public GameObject highlighter;

        private Scratch_MapData mapData;
        private Scratch_AStar aStar;

        private readonly int invalidIndex = -1;
        private int mouseOnTileX = 0;
        private int mouseOnTileZ = 0;
        private int currentTileX;
        private int currentTileZ;

        private bool moving;
        private float fromTileToTile;
        private readonly float durationFromTileToTile = 0.2f;

        // Use this for initialization
        void Start()
        {
            mapData = tileMap.GetComponent<Scratch_Map>().MapData;

            currentTileX = tileMap.GetComponent<Scratch_Map>().MapData.StartingTile.X + 4;
            currentTileZ = tileMap.GetComponent<Scratch_Map>().MapData.StartingTile.Z;

            this.transform.position = new Vector3((float)currentTileX, 0.0f, (float)currentTileZ);

            aStar = new Scratch_AStar(mapData);
        }

        // Update is called once per frame
        void Update()
        {
            ResetMaterials();

            highlighter.transform.position = SetHighlighterPosition();

            currentTileX = (int)(this.transform.position.x + .5f);
            currentTileZ = (int)(this.transform.position.z + .5f);

            if (moving == true) return;

            // 좌클릭
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if ((mouseOnTileX == invalidIndex) || (mouseOnTileZ == invalidIndex)) return;

                bool pathFound = false;
                bool doorTile = false;

                if (mapData.GetTile(mouseOnTileX, mouseOnTileZ).Type == TileType.Door) doorTile = true;

                pathFound = aStar.FindPath(mapData.TileData, mapData.GetTile(currentTileX, currentTileZ), mapData.GetTile(mouseOnTileX, mouseOnTileZ), doorTile);
                
                if (pathFound == false)
                {
                    Debug.Log("Failed to find path.");
                    return;
                }

                if ((aStar.FinalTrack.Count < 2) && (doorTile == true))
                {
                    mapData.GetTile(mouseOnTileX, mouseOnTileZ).OpenDoor();
                    return;
                }

                moving = true;
                StartCoroutine(Move(doorTile, mouseOnTileX, mouseOnTileZ));
            }
        }

        private void ResetMaterials()
        {
            for (int i = 0; i < mapData.Doors.Count; i++)
            {
                if (mapData.Doors[i].Door.Highlighted == true)
                {
                    mapData.Doors[i].Door.ResetDoorColor();
                }
            }
        }

        private IEnumerator Move(bool doorTile = false, int doorX = -1, int doorZ = -1)
        {
            int trackIndex = 0;
            Vector3 startingPosition = new Vector3((float)aStar.FinalTrack[trackIndex].X, 0.0f, (float)aStar.FinalTrack[trackIndex].Z);
            Vector3 destination = new Vector3((float)aStar.FinalTrack[trackIndex + 1].X, 0.0f, (float)aStar.FinalTrack[trackIndex + 1].Z);

            while (true)
            {
                if (fromTileToTile >= durationFromTileToTile)
                {
                    fromTileToTile = 0.0f;

                    if (trackIndex >= aStar.FinalTrack.Count - 2)
                    {
                        if (doorTile == true) mapData.GetTile(doorX, doorZ).OpenDoor();

                        moving = false;
                        break;
                    }

                    trackIndex++;

                    startingPosition = new Vector3((float)aStar.FinalTrack[trackIndex].X, 0.0f, (float)aStar.FinalTrack[trackIndex].Z);
                    destination = new Vector3((float)aStar.FinalTrack[trackIndex + 1].X, 0.0f, (float)aStar.FinalTrack[trackIndex + 1].Z);
                }

                fromTileToTile += Time.deltaTime;
                this.transform.position = Vector3.Lerp(startingPosition, destination, fromTileToTile / durationFromTileToTile);

                yield return null;
            }
        }

        private Vector3 SetHighlighterPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 100.0f, 1 << LayerMask.NameToLayer("Tile Map")) == true)
            {
                mouseOnTileX = (int)(hitInfo.point.x + .5f);
                mouseOnTileZ = (int)(hitInfo.point.z + .5f);

                HighlightDoor();

                Scratch_TileData currentTile = mapData.TileData[mouseOnTileX, mouseOnTileZ];

                return new Vector3((float)currentTile.X, .0f, (float)currentTile.Z);
            }

            mouseOnTileX = invalidIndex;
            mouseOnTileZ = invalidIndex;

            return Vector3.zero;
        }

        private void HighlightDoor()
        {
            if (mapData.GetTile(mouseOnTileX, mouseOnTileZ).Type != TileType.Door) return;

            mapData.GetTile(mouseOnTileX, mouseOnTileZ).Door.HighlightDoor();
        }
    }
}

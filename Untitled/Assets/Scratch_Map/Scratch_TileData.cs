using System.Collections.Generic;

namespace TileData
{
    public enum TileType
    {
        None,
        Floor,
        Door,
        DoorWall,
        Wall
    }

    public enum WallDirection
    {
        None,
        Left,
        Right,
        Here,
        There
    }

    public class Scratch_TileData
    {
        public Scratch_TileData(int x, int z)
        {
            X = x;
            Z = z;
            Type = TileType.None;
            //DoorOpened = false;
            Direction = new List<WallDirection>();
        }

        public int X { get; private set; }
        public int Z { get; private set; }
        public TileType Type { get; private set; }
        //public bool DoorOpened { get; private set; }
        public Scratch_Door Door { get; set; }
        // Direction of wall / door
        public List<WallDirection> Direction { get; private set; }

        public void OpenDoor()
        {
            //DoorOpened = true;
            Type = TileType.Floor;
            Door.Open();
        }

        public void HighlightDoor()
        {
            // 이 타일이 가진 문을 highlight
        }

        public void UpdateTile(TileType newType)
        {
            Type = newType;
        }

        public void UpdateDoorDirection(WallDirection doorDirection)
        {
            Direction.Add(doorDirection);
        }

        public void UpdateWallDirection(int wallX, int wallZ, int adjacentFloorX, int adjacentFloorZ)
        {
            int x = adjacentFloorX - wallX;
            int z = adjacentFloorZ - wallZ;

            WallDirection wallDirection;

            // Left or right
            if (z == 0)
            {
                wallDirection = (x > 0) ? WallDirection.Left : WallDirection.Right;
            }
            // Here or there
            else
            {
                wallDirection = (z > 0) ? WallDirection.Here : WallDirection.There;
            }

            Direction.Add(wallDirection);
        }
    }
}

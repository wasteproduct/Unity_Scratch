using System.Collections.Generic;

namespace TileData
{
    public enum TileType
    {
        None,
        Floor,
        Door,
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
        public Scratch_TileData()
        {
            Type = TileType.None;
            DoorOpened = false;
            Direction = new List<WallDirection>();
        }

        public TileType Type { get; private set; }
        public bool DoorOpened { get; private set; }
        // Direction of wall
        public List<WallDirection> Direction { get; private set; }

        public void UpdateTile(TileType newType)
        {
            Type = newType;
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

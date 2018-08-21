namespace TileData
{
    public enum TileType
    {
        None,
        Floor,
        Wall
    }

    public class Room
    {
        private readonly int width, height;

        public Room(int startingTileX, int startingTileZ, int roomWidth, int roomHeight)
        {
            X = startingTileX;
            Z = startingTileZ;

            width = roomWidth;
            height = roomHeight;

            Right = X + width - 1;
            Top = Z + height - 1;
        }

        public int X { get; private set; }
        public int Z { get; private set; }
        public int Right { get; private set; }
        public int Top { get; private set; }

        public bool RoomsOverlapping(Room room)
        {
            if (this.X > room.Right + 1) return false;
            if (this.Right < room.X - 1) return false;
            if (this.Z > room.Top + 1) return false;
            if (this.Top < room.Z - 1) return false;

            return true;
        }
    }

    public class Scratch_TileData
    {
        public Scratch_TileData()
        {
            Type = TileType.None;
        }

        public TileType Type { get; private set; }

        public void UpdateTile(TileType newType)
        {
            Type = newType;
        }
    }
}

namespace RoomData
{
    //public enum AreaIdentity
    //{
    //    Room,
    //    Hallway
    //}

    //public class MapArea
    //{
    //    public MapArea(int x, int z)
    //    {
    //        Identity = AreaIdentity.Hallway;

    //        X = x;
    //        Z = z;
    //    }

    //    public AreaIdentity Identity { get; set; }
    //    public int X { get; private set; }
    //    public int Z { get; private set; }
    //}

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

            CenterX = X + width / 2;
            CenterZ = Z + height / 2;

            Connected = false;
        }

        public int X { get; private set; }
        public int Z { get; private set; }
        public int Right { get; private set; }
        public int Top { get; private set; }
        public int CenterX { get; private set; }
        public int CenterZ { get; private set; }
        public bool Connected { get; set; }

        public bool RoomsOverlapping(Room room)
        {
            if (this.X > room.Right + 1) return false;
            if (this.Right < room.X - 1) return false;
            if (this.Z > room.Top + 1) return false;
            if (this.Top < room.Z - 1) return false;

            return true;
        }
    }
}

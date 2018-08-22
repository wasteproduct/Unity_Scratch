namespace TileData
{
    public enum TileType
    {
        None,
        Floor,
        Door,
        Wall
    }

    public class Scratch_TileData
    {
        public Scratch_TileData()
        {
            Type = TileType.None;
            DoorOpened = false;
        }

        public TileType Type { get; private set; }
        public bool DoorOpened { get; private set; }

        public void UpdateTile(TileType newType)
        {
            Type = newType;
        }
    }
}

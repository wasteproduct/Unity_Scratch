namespace TileData
{
    public enum TileType
    {
        None,
        Floor,
        Wall
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

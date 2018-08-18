public class Omok_BoardTile {
    public enum TileOccupation
    {
        None,
        Black,
        White
    }

    public Omok_BoardTile()
    {
        Occupier = TileOccupation.None;
    }

    public TileOccupation Occupier { get; set; }
}

using UnityEngine;
using System.Collections.Generic;
using RoomData;
using TileData;

namespace MapData
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
    public enum MapSize
    {
        Small,
        Medium,
        Large
    }

    public class Scratch_MapData
    {
        private readonly int areaTilesRow, areaTilesColumn;

        public Scratch_MapData(MapSize mapSize)
        {
            areaTilesRow = areaTilesColumn = 32;

            int areasRow = 0;
            int areasColumn = 0;

            SetAreas(areasRow, areasColumn, mapSize);

            SetTileData(areasRow, areasColumn);

            RoomMetaData roomsMetaData = new RoomMetaData();
            
        }

        public int TilesRow { get; private set; }
        public int TilesColumn { get; private set; }
        public Scratch_TileData[,] TileData { get; private set; }

        private void SetTileData(int areasRow, int areasColumn)
        {
            TilesRow = areaTilesRow * areasRow;
            TilesColumn = areaTilesColumn * areasColumn;

            TileData = new Scratch_TileData[TilesRow, TilesColumn];
            for (int z = 0; z < TilesColumn; z++)
            {
                for (int x = 0; x < TilesRow; x++)
                {
                    TileData[x, z] = new Scratch_TileData();
                }
            }
        }

        private void SetAreas(int areasRow, int areasColumn, MapSize mapSize)
        {
            switch (mapSize)
            {
                case MapSize.Small:
                    areasRow = areasColumn = 2;
                    break;
                case MapSize.Medium:
                    areasRow = areasColumn = 3;
                    break;
                case MapSize.Large:
                    areasRow = areasColumn = 4;
                    break;
            }
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using RoomData;
using TileData;

namespace MapData
{
    public enum MapSize
    {
        Small,
        Medium,
        Large
    }

    public class MapArea
    {
        public MapArea(int x, int z)
        {
            X = x;
            Z = z;
        }
        
        public int X { get; private set; }
        public int Z { get; private set; }
    }

    public class Scratch_MapData
    {
        private readonly int areaTilesRow, areaTilesColumn;

        public Scratch_MapData(MapSize mapSize)
        {
            areaTilesRow = areaTilesColumn = 32;

            int areasRow = 0;
            int areasColumn = 0;

            MapArea[,] area = SetAreas(ref areasRow, ref areasColumn, mapSize);
            
            SetTileData(areasRow, areasColumn);

            RoomMetaData roomsMetaData = new RoomMetaData();

            CreateRooms(areasRow, areasColumn, area, roomsMetaData);
        }

        public int TilesRow { get; private set; }
        public int TilesColumn { get; private set; }
        public Scratch_TileData[,] TileData { get; private set; }

        private void CreateRooms(int areasRow, int areasColumn, MapArea[,] area, RoomMetaData roomsMetaData)
        {
            List<Room> rooms = new List<Room>();

            for (int z = 0; z < areasColumn; z++)
            {
                for (int x = 0; x < areasRow; x++)
                {
                    int roomX = Random.Range(area[x, z].X + 3, area[x, z].X + 8);
                    int roomZ = Random.Range(area[x, z].Z + 3, area[x, z].Z + 8);
                    int roomWidth = Random.Range(roomsMetaData.MinimumRoomWidth, roomsMetaData.MaximumRoomWidth);
                    int roomHeight = Random.Range(roomsMetaData.MinimumRoomHeight, roomsMetaData.MaximumRoomHeight);

                    Room newRoom = new Room(roomX, roomZ, roomWidth, roomHeight);

                    rooms.Add(newRoom);
                }
            }

            SetRooms(rooms);

            ConnectRooms(rooms);
        }

        private void ConnectRooms(List<Room> rooms)
        {
            for (int i = 0; i < rooms.Count - 1; i++)
            {
                if (rooms[i].Connected == true) continue;

                int nextRoom = i + 1;

                int x = rooms[i].CenterX;
                int z = rooms[i].CenterZ;

                int xIncrementor = (x < rooms[nextRoom].CenterX) ? 1 : -1;
                int zIncrementor = (z < rooms[nextRoom].CenterZ) ? 1 : -1;

                while (true)
                {
                    if (x == rooms[nextRoom].CenterX) break;

                    x += xIncrementor;

                    if (TileData[x, z].Type == TileType.Floor) continue;

                    TileData[x, z].UpdateTile(TileType.Floor);

                    int upperZ = z + 1;
                    if (upperZ < TilesColumn) TileData[x, upperZ].UpdateTile(TileType.Floor);

                    int lowerZ = z - 1;
                    if (lowerZ >= 0) TileData[x, lowerZ].UpdateTile(TileType.Floor);
                }

                while (true)
                {
                    if (z == rooms[nextRoom].CenterZ) break;

                    z += zIncrementor;

                    if (TileData[x, z].Type == TileType.Floor) continue;

                    TileData[x, z].UpdateTile(TileType.Floor);

                    int leftX = x - 1;
                    if (leftX >= 0) TileData[leftX, z].UpdateTile(TileType.Floor);

                    int rightX = x + 1;
                    if (rightX < TilesRow) TileData[rightX, z].UpdateTile(TileType.Floor);
                }

                rooms[i].Connected = true;
            }
        }

        private void SetRooms(List<Room> rooms)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                for (int z = rooms[i].Z; z <= rooms[i].Top; z++)
                {
                    for (int x = rooms[i].X; x <= rooms[i].Right; x++)
                    {
                        TileData[x, z].UpdateTile(TileType.Floor);
                    }
                }
            }
        }

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

        private MapArea[,] SetAreas(ref int areasRow, ref int areasColumn, MapSize mapSize)
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

            MapArea[,] area = new MapArea[areasRow, areasColumn];

            for (int z = 0; z < areasColumn; z++)
            {
                for (int x = 0; x < areasRow; x++)
                {
                    if (z % 2 == 0) area[x, z] = new MapArea(x * areaTilesRow, z * areaTilesColumn);
                    else area[x, z] = new MapArea((areasRow - 1 - x) * areaTilesRow, z * areaTilesColumn);
                }
            }

            return area;
        }
    }
}

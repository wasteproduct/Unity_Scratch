using UnityEngine;
using System.Collections.Generic;
using RoomData;
using TileData;

namespace MapData
{
    public class Scratch_MapData
    {
        private readonly int row, column;

        public Scratch_MapData(int tilesRow, int tilesColumn)
        {
            row = tilesRow;
            column = tilesColumn;

            SetTileData();

            CreateRooms();
        }

        public Scratch_TileData[,] TileData { get; private set; }

        private void CreateRooms()
        {

        }

        private void SetTileData()
        {
            TileData = new Scratch_TileData[row, column];

            for (int z = 0; z < column; z++)
            {
                for (int x = 0; x < row; x++)
                {
                    TileData[x, z] = new Scratch_TileData();
                }
            }
        }

        //private void CreateRooms()
        //{
        //    //int areasRow = row / 10;
        //    //int areasColumn = column / 10;
        //    //int areaWidth = row / areasRow;
        //    //int areaHeight = column / areasColumn;
        //    //int roomsNumber = areasRow + areasColumn;

        //    //MapArea[,] area = SetAreas(areasRow, areasColumn, areaWidth, areaHeight, roomsNumber);

        //    //List<Room> rooms = new List<Room>();

        //    //for (int z = 0; z < areasColumn; z++)
        //    //{
        //    //    for (int x = 0; x < areasRow; x++)
        //    //    {
        //    //        if (area[x, z].Identity == AreaIdentity.Hallway) continue;

        //    //        int roomWidth = Random.Range((int)((float)areasRow * .6f), (int)((float)areasRow * .9f));
        //    //        int roomHeight = Random.Range((int)((float)areasColumn * .6f), (int)((float)areasColumn * .9f));
        //    //        int roomX = Random.Range(area[x, z].X, area[x, z].X + areaWidth - 1 - roomWidth);
        //    //        int roomZ = Random.Range(area[x, z].Z, area[x, z].Z + areaHeight - 1 - roomHeight);

        //    //        Room newRoom = new Room(roomX, roomZ, roomWidth, roomHeight);

        //    //        rooms.Add(newRoom);
        //    //    }
        //    //}

        //    //SetRooms(rooms);
        //}

        ////private void SetRooms(List<Room> rooms)
        ////{
        ////    for (int i = 0; i < rooms.Count; i++)
        ////    {
        ////        for (int z = rooms[i].Z; z <= rooms[i].Top; z++)
        ////        {
        ////            for (int x = rooms[i].X; x <= rooms[i].Right; x++)
        ////            {
        ////                TileData[x, z].UpdateTile(TileType.Floor);
        ////            }
        ////        }
        ////    }
        ////}

        ////private MapArea[,] SetAreas(int areasRow, int areasColumn, int areaWidth, int areaHeight, int roomsNumber)
        ////{
        ////    MapArea[,] area = new MapArea[areasRow, areasColumn];
        ////    for (int z = 0; z < areasColumn; z++)
        ////    {
        ////        for (int x = 0; x < areasRow; x++)
        ////        {
        ////            area[x, z] = new MapArea(x * areaWidth, z * areaHeight);
        ////        }
        ////    }

        ////    int roomsCount = 0;
        ////    while (true)
        ////    {
        ////        int x = Random.Range(0, areasRow);
        ////        int z = Random.Range(0, areasColumn);

        ////        if (area[x, z].Identity == AreaIdentity.Room) continue;

        ////        area[x, z].Identity = AreaIdentity.Room;

        ////        roomsCount++;

        ////        if (roomsCount >= roomsNumber) break;
        ////    }

        ////    return area;
        ////}

        ////private void ConnectRooms(List<Room> rooms)
        ////{

        ////}

        //////private void ConnectRooms(List<Room> rooms)
        //////{
        //////    for (int i = 0; i < rooms.Count - 1; i++)
        //////    {
        //////        if (rooms[i].Connected == true) continue;

        //////        int x = rooms[i].CenterX;
        //////        int z = rooms[i].CenterZ;

        //////        int xIncrementor = (x < rooms[i + 1].CenterX) ? 1 : -1;
        //////        int zIncrementor = (z < rooms[i + 1].CenterZ) ? 1 : -1;

        //////        while (true)
        //////        {
        //////            if (TileData[x, z].Type != TileType.Floor)
        //////            {
        //////                TileData[x, z].UpdateTile(TileType.Floor);

        //////                int upperZ = z + 1;
        //////                if (upperZ < column) TileData[x, upperZ].UpdateTile(TileType.Floor);

        //////                int lowerZ = z - 1;
        //////                if (lowerZ >= 0) TileData[x, lowerZ].UpdateTile(TileType.Floor);
        //////            }

        //////            if (x == rooms[i + 1].CenterX) break;

        //////            x += xIncrementor;
        //////        }

        //////        while (true)
        //////        {
        //////            if (TileData[x, z].Type != TileType.Floor)
        //////            {
        //////                TileData[x, z].UpdateTile(TileType.Floor);

        //////                int leftX = x - 1;
        //////                if (leftX >= 0) TileData[leftX, z].UpdateTile(TileType.Floor);

        //////                int rightX = x + 1;
        //////                if (rightX < row) TileData[rightX, z].UpdateTile(TileType.Floor);
        //////            }

        //////            if (z == rooms[i + 1].CenterZ) break;

        //////            z += zIncrementor;
        //////        }

        //////        rooms[i].Connected = true;
        //////    }
        //////}
    }
}

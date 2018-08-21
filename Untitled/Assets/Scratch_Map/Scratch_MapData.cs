using UnityEngine;
using System.Collections.Generic;
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
            List<Room> rooms = new List<Room>();

            int maximumRooms = row / 10 + column / 10;

            if (maximumRooms <= 0) return;

            int maximumRoomWidth = (row / 10) * 3;
            int minimumRoomWidth = (row / 10) * 2;
            int maximumRoomHeight = (column / 10) * 3;
            int minimumRoomHeight = (column / 10) * 2;

            int failureCount = row + column;

            while (true)
            {
                failureCount--;
                if (failureCount <= 0) break;

                int x = Random.Range(0, row - maximumRoomWidth);
                int z = Random.Range(0, column - maximumRoomHeight);

                int roomWidth = Random.Range(minimumRoomWidth, maximumRoomWidth);
                int roomHeight = Random.Range(minimumRoomHeight, maximumRoomHeight);

                Room newRoom = new Room(x, z, roomWidth, roomHeight);

                bool roomsOverlapping = false;
                for (int i = 0; i < rooms.Count; i++)
                {
                    roomsOverlapping = newRoom.RoomsOverlapping(rooms[i]);

                    if (roomsOverlapping == true) break;
                }

                if (roomsOverlapping == true) continue;

                rooms.Add(newRoom);

                if (rooms.Count >= maximumRooms) break;
            }

            SetRooms(rooms);
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

            ConnectRooms(rooms);
        }

        private void ConnectRooms(List<Room> rooms)
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
    }
}

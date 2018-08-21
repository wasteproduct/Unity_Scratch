using UnityEngine;
using System.Collections.Generic;
using TileData;

namespace MapData
{
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
            for (int i = 0; i < rooms.Count - 1; i++)
            {
                if (rooms[i].Connected == true) continue;

                int x = rooms[i].CenterX;
                int z = rooms[i].CenterZ;

                int xIncrementor = (x < rooms[i + 1].CenterX) ? 1 : -1;
                int zIncrementor = (z < rooms[i + 1].CenterZ) ? 1 : -1;

                while (true)
                {
                    if (TileData[x, z].Type != TileType.Floor) TileData[x, z].UpdateTile(TileType.Floor);

                    if (x == rooms[i + 1].CenterX) break;

                    x += xIncrementor;
                }

                while (true)
                {
                    if (TileData[x, z].Type != TileType.Floor) TileData[x, z].UpdateTile(TileType.Floor);

                    if (z == rooms[i + 1].CenterZ) break;

                    z += zIncrementor;
                }

                rooms[i].Connected = true;
            }
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

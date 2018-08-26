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
        private readonly int areasRow, areasColumn;
        private readonly int invalidIndex = -1;

        public Scratch_MapData(MapSize mapSize)
        {
            areaTilesRow = areaTilesColumn = 32;

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

            List<MapArea> area = SetAreas(mapSize);

            SetTileData();

            List<Room> rooms = CreateRooms(area);

            SetRooms(rooms);

            SetDoors(area, rooms);

            ConnectRooms(rooms);

            SetWalls();
        }

        public int TilesRow { get; private set; }
        public int TilesColumn { get; private set; }
        public Scratch_TileData[,] TileData { get; private set; }
        public List<Scratch_TileData> Doors { get; private set; }

        private void SetWalls()
        {
            for (int z = 0; z < TilesColumn; z++)
            {
                for (int x = 0; x < TilesRow; x++)
                {
                    if ((TileData[x, z].Type == TileType.Floor) || (TileData[x, z].Type == TileType.Door)) continue;

                    int upper = z + 1;
                    int lower = z - 1;
                    int left = x - 1;
                    int right = x + 1;

                    if (ColumnIndexAvailable(upper) == true)
                    {
                        if (TileData[x, upper].Type == TileType.Floor)
                        {
                            TileData[x, z].UpdateTile(TileType.Wall);
                            TileData[x, z].UpdateWallDirection(x, z, x, upper);
                        }
                    }

                    if (ColumnIndexAvailable(lower) == true)
                    {
                        if (TileData[x, lower].Type == TileType.Floor)
                        {
                            TileData[x, z].UpdateTile(TileType.Wall);
                            TileData[x, z].UpdateWallDirection(x, z, x, lower);
                        }
                    }

                    if (RowIndexAvailable(left) == true)
                    {
                        if (TileData[left, z].Type == TileType.Floor)
                        {
                            TileData[x, z].UpdateTile(TileType.Wall);
                            TileData[x, z].UpdateWallDirection(x, z, left, z);
                        }
                    }

                    if (RowIndexAvailable(right) == true)
                    {
                        if (TileData[right, z].Type == TileType.Floor)
                        {
                            TileData[x, z].UpdateTile(TileType.Wall);
                            TileData[x, z].UpdateWallDirection(x, z, right, z);
                        }
                    }
                }
            }

            for(int i=0;i<Doors.Count;i++)
            {
                switch(Doors[i].Direction[0])
                {
                    case WallDirection.Left:
                        TileData[Doors[i].X, Doors[i].Z - 1].UpdateTile(TileType.DoorWall);
                        TileData[Doors[i].X, Doors[i].Z + 1].UpdateTile(TileType.DoorWall);
                        break;
                    case WallDirection.Right:
                        TileData[Doors[i].X, Doors[i].Z + 1].UpdateTile(TileType.DoorWall);
                        TileData[Doors[i].X, Doors[i].Z - 1].UpdateTile(TileType.DoorWall);
                        break;
                    case WallDirection.Here:
                        TileData[Doors[i].X + 1, Doors[i].Z].UpdateTile(TileType.DoorWall);
                        TileData[Doors[i].X - 1, Doors[i].Z].UpdateTile(TileType.DoorWall);
                        break;
                    case WallDirection.There:
                        TileData[Doors[i].X - 1, Doors[i].Z].UpdateTile(TileType.DoorWall);
                        TileData[Doors[i].X + 1, Doors[i].Z].UpdateTile(TileType.DoorWall);
                        break;
                }
            }
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

                    int upperZ = z + 1;
                    if (upperZ < TilesColumn) TileData[x, upperZ].UpdateTile(TileType.Floor);

                    int lowerZ = z - 1;
                    if (lowerZ >= 0) TileData[x, lowerZ].UpdateTile(TileType.Floor);

                    if (TileData[x, z].Type == TileType.Door) continue;
                    TileData[x, z].UpdateTile(TileType.Floor);
                }

                while (true)
                {
                    if (z == rooms[nextRoom].CenterZ) break;

                    z += zIncrementor;

                    if (TileData[x, z].Type == TileType.Floor) continue;

                    int leftX = x - 1;
                    if (leftX >= 0) TileData[leftX, z].UpdateTile(TileType.Floor);

                    int rightX = x + 1;
                    if (rightX < TilesRow) TileData[rightX, z].UpdateTile(TileType.Floor);

                    if (TileData[x, z].Type == TileType.Door) continue;
                    TileData[x, z].UpdateTile(TileType.Floor);
                }

                rooms[i].Connected = true;
            }
        }

        private void SetDoors(List<MapArea> area, List<Room> rooms)
        {
            Doors = new List<Scratch_TileData>();

            int current = 0;

            while (true)
            {
                SetDoorToPreviousRoom(current - 1, current, area, rooms);
                SetDoorToNextRoom(current, current + 1, area, rooms);

                current++;

                if (current >= area.Count) break;
            }
        }

        private void SetDoorToNextRoom(int current, int next, List<MapArea> area, List<Room> rooms)
        {
            if (next >= area.Count) return;

            const int horizontal = -1;
            const int vertical = 1;

            int direction = 0;
            direction = (area[current].Z == area[next].Z) ? horizontal : vertical;

            int doorX = invalidIndex;
            int doorZ = invalidIndex;
            WallDirection doorDirection = WallDirection.None;

            switch (direction)
            {
                case horizontal:
                    doorDirection = (area[current].X < area[next].X) ? WallDirection.Right : WallDirection.Left;
                    break;
                case vertical:
                    doorDirection = WallDirection.There;
                    break;
                default:
                    Debug.Log("Direction was decided unsuccessfully.");
                    return;
            }

            switch (doorDirection)
            {
                case WallDirection.Left:
                    doorX = rooms[current].X - 1;
                    doorZ = rooms[current].CenterZ;
                    break;
                case WallDirection.Right:
                    doorX = rooms[current].Right + 1;
                    doorZ = rooms[current].CenterZ;
                    break;
                case WallDirection.There:
                    doorX = rooms[next].CenterX;
                    doorZ = rooms[current].Top + 1;
                    break;
            }

            if ((doorX == invalidIndex) || (doorZ == invalidIndex))
            {
                Debug.Log("Door indices were set unsuccessfully.");
                return;
            }

            TileData[doorX, doorZ].UpdateTile(TileType.Door);
            TileData[doorX, doorZ].UpdateDoorDirection(doorDirection);
            Doors.Add(TileData[doorX, doorZ]);
        }

        private void SetDoorToPreviousRoom(int previous, int current, List<MapArea> area, List<Room> rooms)
        {
            if (previous < 0) return;

            const int horizontal = -1;
            const int vertical = 1;

            int direction = 0;
            direction = (area[previous].Z == area[current].Z) ? horizontal : vertical;

            int doorX = invalidIndex;
            int doorZ = invalidIndex;
            WallDirection doorDirection = WallDirection.None;

            switch (direction)
            {
                case horizontal:
                    doorDirection = (area[previous].X < area[current].X) ? WallDirection.Left : WallDirection.Right;
                    break;
                case vertical:
                    doorDirection = WallDirection.Here;
                    break;
                default:
                    Debug.Log("Direction was decided unsuccessfully.");
                    return;
            }

            switch(doorDirection)
            {
                case WallDirection.Left:
                    doorX = rooms[current].X - 1;
                    doorZ = rooms[previous].CenterZ;
                    break;
                case WallDirection.Right:
                    doorX = rooms[current].Right + 1;
                    doorZ = rooms[previous].CenterZ;
                    break;
                case WallDirection.Here:
                    doorX = rooms[current].CenterX;
                    doorZ = rooms[current].Z - 1;
                    break;
            }

            if ((doorX == invalidIndex) || (doorZ == invalidIndex))
            {
                Debug.Log("Door indices were set unsuccessfully.");
                return;
            }

            TileData[doorX, doorZ].UpdateTile(TileType.Door);
            TileData[doorX, doorZ].UpdateDoorDirection(doorDirection);
            Doors.Add(TileData[doorX, doorZ]);
        }

        private List<Room> CreateRooms(List<MapArea> area)
        {
            List<Room> rooms = new List<Room>();

            RoomMetaData roomsMetaData = new RoomMetaData();

            for (int i = 0; i < area.Count; i++)
            {
                int roomX = Random.Range(area[i].X + 3, area[i].X + 8);
                int roomZ = Random.Range(area[i].Z + 3, area[i].Z + 8);
                int roomWidth = Random.Range(roomsMetaData.MinimumRoomWidth, roomsMetaData.MaximumRoomWidth);
                int roomHeight = Random.Range(roomsMetaData.MinimumRoomHeight, roomsMetaData.MaximumRoomHeight);

                Room newRoom = new Room(roomX, roomZ, roomWidth, roomHeight);

                rooms.Add(newRoom);
            }

            return rooms;
        }

        private bool RowIndexAvailable(int x)
        {
            if ((x < 0) || (x >= TilesRow)) return false;

            return true;
        }

        private bool ColumnIndexAvailable(int z)
        {
            if ((z < 0) || (z >= TilesColumn)) return false;

            return true;
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

        private void SetTileData()
        {
            TilesRow = areaTilesRow * areasRow;
            TilesColumn = areaTilesColumn * areasColumn;

            TileData = new Scratch_TileData[TilesRow, TilesColumn];
            for (int z = 0; z < TilesColumn; z++)
            {
                for (int x = 0; x < TilesRow; x++)
                {
                    TileData[x, z] = new Scratch_TileData(x, z);
                }
            }
        }

        private List<MapArea> SetAreas(MapSize mapSize)
        {
            List<MapArea> area = new List<MapArea>();

            for (int z = 0; z < areasColumn; z++)
            {
                for (int x = 0; x < areasRow; x++)
                {
                    int areaX = (z % 2 == 0) ? x * areaTilesRow : (areasRow - 1 - x) * areaTilesRow;
                    int areaZ = z * areaTilesColumn;

                    MapArea newArea = new MapArea(areaX, areaZ);

                    area.Add(newArea);
                }
            }

            return area;
        }
    }
}

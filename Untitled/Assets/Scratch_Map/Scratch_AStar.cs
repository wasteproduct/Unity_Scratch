using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class Scratch_AStar
    {
        private readonly int row, column;
        private Node_AStar currentNode;
        private Node_AStar[,] node;

        public Calculation_AStar(TileMap_MapData mapData)
        {
            row = mapData.row;
            column = mapData.column;

            currentNode = null;

            node = new Node_AStar[row, column];

            for (int z = 0; z < column; z++)
            {
                for (int x = 0; x < row; x++)
                {
                    node[x, z] = new Node_AStar(mapData.TileData[x, z]);
                }
            }

            openList = new List<Node_AStar>();
            closedList = new List<Node_AStar>();

            FinalTrack = new List<Node_AStar>();
        }

        public List<Node_AStar> FinalTrack { get; private set; }

        public bool FindPath(TileMap_MapData.TileMap_TileData[,] tileData, TileMap_MapData.TileMap_TileData startingTile, TileMap_MapData.TileMap_TileData destinationTile)
        {
            Refresh(tileData, destinationTile);

            currentNode = node[startingTile.Index.x, startingTile.Index.z];

            closedList.Add(currentNode);

            int failureCount = row * column;

            while (true)
            {
                for (int z = currentNode.Index.z - 1; z < currentNode.Index.z + 2; z++)
                {
                    for (int x = currentNode.Index.x - 1; x < currentNode.Index.x + 2; x++)
                    {
                        if (NodeIndexAvailable(x, z) == false) continue;

                        if (node[x, z].Passable == false) continue;

                        if (NodeInClosedList(node[x, z]) == true) continue;

                        if (NodeInOpenList(node[x, z]) == false)
                        {
                            node[x, z].Parent = currentNode;
                            node[x, z].CalculateCostToDestination();
                            openList.Add(node[x, z]);
                        }
                        else
                        {
                            Node_AStar newData = new Node_AStar(node[x, z]);
                            newData.Parent = currentNode;
                            newData.CalculateCostToDestination();

                            if (newData.CostToDestination < node[x, z].CostToDestination)
                            {
                                node[x, z].Parent = currentNode;
                                node[x, z].CalculateCostToDestination();
                            }
                        }
                    }
                }

                int lowestCost = 99999999;
                for (int i = 0; i < openList.Count; i++)
                {
                    if (openList[i].CostToDestination < lowestCost)
                    {
                        lowestCost = openList[i].CostToDestination;
                        currentNode = openList[i];
                    }
                }

                if (currentNode == node[destinationTile.Index.x, destinationTile.Index.z])
                {
                    int whileBreaker = row * column;

                    while (true)
                    {
                        FinalTrack.Add(currentNode);

                        if (currentNode == node[startingTile.Index.x, startingTile.Index.z]) return true;

                        currentNode = currentNode.Parent;

                        whileBreaker--;
                        if (whileBreaker < 0) return false;
                    }
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                failureCount--;
                if (failureCount < 0) return false;
            }
        }

        private bool NodeInOpenList(Node_AStar checkedNode)
        {
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i] == checkedNode) return true;
            }

            return false;
        }

        private void Refresh(TileMap_MapData.TileMap_TileData[,] tileData, TileMap_MapData.TileMap_TileData destinationTile)
        {
            openList.Clear();
            closedList.Clear();
            FinalTrack.Clear();
            currentNode = null;

            for (int z = 0; z < column; z++)
            {
                for (int x = 0; x < row; x++)
                {
                    node[x, z].Initialize(tileData[x, z], destinationTile);
                }
            }
        }

        private bool NodeInClosedList(Node_AStar checkedNode)
        {
            for (int i = 0; i < closedList.Count; i++)
            {
                if (closedList[i] == checkedNode) return true;
            }

            return false;
        }

        private bool NodeIndexAvailable(int x, int z)
        {
            if ((x < 0) || (x >= row) || (z < 0) || (z >= column)) return false;

            return true;
        }
    }

    public class Node_AStar
    {
        public Node_AStar(TileMap_MapData.TileMap_TileData correspondingTile)
        {
            Index = correspondingTile.Index;
        }

        public Node_AStar(Node_AStar copiedNode)
        {
            this.Index = copiedNode.Index;
            this.Parent = copiedNode.Parent;
            this.Passable = copiedNode.Passable;
            this.DistanceFromStart = copiedNode.DistanceFromStart;
            this.DistanceToDestination = copiedNode.DistanceToDestination;
            this.CostToDestination = copiedNode.CostToDestination;
        }

        public TileMap_MapData.TileIndex Index { get; private set; }
        public Node_AStar Parent { get; set; }
        public bool Passable { get; private set; }
        public int DistanceFromStart { get; private set; }
        public int DistanceToDestination { get; private set; }
        public int CostToDestination { get; private set; }

        public void Initialize(TileMap_MapData.TileMap_TileData correspondingTile, TileMap_MapData.TileMap_TileData destinationTile)
        {
            Parent = null;
            Passable = correspondingTile.Passable;

            CalculateDistanceToDestination(destinationTile);
        }

        public void CalculateCostToDestination()
        {
            CalculateDistanceFromStart();

            CostToDestination = DistanceFromStart + DistanceToDestination;
        }

        private void CalculateDistanceFromStart()
        {
            if ((this.Parent.Index.x - this.Index.x != 0) && (this.Parent.Index.z - this.Index.z != 0))
            {
                this.DistanceFromStart = this.Parent.DistanceFromStart + 14;
            }
            else
            {
                this.DistanceFromStart = this.Parent.DistanceFromStart + 10;
            }
        }

        private void CalculateDistanceToDestination(TileMap_MapData.TileMap_TileData destinationTile)
        {
            int destinationX = destinationTile.Index.x;
            int destinationZ = destinationTile.Index.z;

            int xDistance = Mathf.Abs(destinationX - Index.x);
            int zDistance = Mathf.Abs(destinationZ - Index.z);

            if (xDistance - zDistance == 0)
            {
                DistanceToDestination = 14 * zDistance;
            }
            else
            {
                int linearDistance = Mathf.Abs(xDistance - zDistance);
                int furtherAxis = (xDistance - zDistance > 0) ? xDistance : zDistance;
                int diagonalDistance = furtherAxis - linearDistance;

                DistanceToDestination = linearDistance * 10 + diagonalDistance * 14;
            }
        }
    }
}

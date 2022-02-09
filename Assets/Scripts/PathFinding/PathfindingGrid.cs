﻿using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

namespace Astar2DPathFinding.Mika
{
    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }

    public class PathfindingGrid : Singleton<PathfindingGrid>
    {
        [SerializeField] private Vector2 gridWorldSize = new Vector2(100, 100);
        [SerializeField] private float nodeRadius = 1;

        private float nodeDiameter
        {
            get { return nodeRadius * 2; }
        }

        [SerializeField] private float nearestNodeDistance = 10;
        [SerializeField] private float collisionRadius = 1;

        public Node[,] grid;
        private int gridSizeX, gridSizeY;


        public LayerMask unwalkableMask;
        public TerrainType[] walkableRegions;
        private LayerMask walkableMask;
        Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();


        public Connections connectionsOptions;
        public Heuristics heuristicMethod;

        //Using this value can decide whether algoritmin should work more like dijkstra or greedy best first. If value is 1 this works like traditional A*.
        public float heuristicMultiplier;
        public bool showGrid;
        public bool showPathSearchDebug;

        public enum Connections
        {
            directional4,
            directional8,
            directional8DontCutCorners
        }

        public enum Heuristics
        {
            VectorMagnitude,
            Manhattan,
            Euclidean
        }

        //This is for showing calculated path. This can be used to debug paths. Can be removed.
        public static List<Node> openList = new List<Node>();
        public static List<Node> closedList = new List<Node>();
        public static bool pathFound;


        public static int Maxsize
        {
            get { return instance.gridSizeX * instance.gridSizeY; }
        }

        public override void Awake()
        {
            base.Awake();

            AddWalkableRegionsToDictonary();

            CreateGrid();
        }

        private void AddWalkableRegionsToDictonary()
        {
            foreach (TerrainType region in walkableRegions)
            {
                walkableMask.value |= region.terrainMask.value;
                int terrainMask = (int)Mathf.Log(region.terrainMask.value, 2);
                if (walkableRegionsDictionary.ContainsKey(terrainMask))
                {
                    walkableRegionsDictionary[terrainMask] = region.terrainPenalty;
                }
                else
                {
                    walkableRegionsDictionary.Add(terrainMask, region.terrainPenalty);
                }
            }
        }


        public void CreateGrid()
        {
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

            grid = new Node[gridSizeX, gridSizeY];
            Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 -
                                      Vector2.up * gridWorldSize.y / 2;
            int walk = 0;
            int obs = 0;
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) +
                                         Vector2.up * (y * nodeDiameter + nodeRadius);

                    bool walkable =
                        (Physics2D.OverlapCircle(worldPoint, nodeRadius * collisionRadius, unwalkableMask) == null);
                    NodeType nodeType = NodeType.obstacle;
                    if (walkable)
                    {
                        walk++;
                        nodeType = NodeType.walkable;
                    }
                    else
                    {
                        obs++;
                        nodeType = NodeType.obstacle;
                    }

                    int movementPenalty = 0;

                    Collider2D[] hit = Physics2D.OverlapCircleAll(worldPoint, nodeRadius, walkableMask);
                    //RaycastHit2D hits = Physics2D.CircleCast(worldPoint, nodeRadius);
                    for (int i = 0; i < hit.Length; i++)
                    {
                        int newPenalty = 0;

                        walkableRegionsDictionary.TryGetValue(hit[i].gameObject.layer, out newPenalty);

                        //Return terrain with highest movement penalty
                        if (newPenalty > movementPenalty)
                        {
                            movementPenalty = newPenalty;
                        }
                    }
                    /*if (hits.collider.CompareTag("Wall"))
                    {
                        nodeType = NodeType.obstacle;
                    }*/
                    grid[x, y] = new Node(nodeType, worldPoint, x, y, movementPenalty);
                }
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    GetNeighbours(grid[x, y]);
                }
            }

            sw.Stop();
            print("Walk: " + walk + " Obs: " + obs + "Time took create the grid" + sw.Elapsed);
            SetAreas();
        }
        
        int currentIDThing = 1;

        public void SetAreas()
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    if (grid[x, y].walkable != NodeType.obstacle && grid[x, y].gridAreaID == 0)
                    {
                        SetGridAreas(grid[x, y], currentIDThing);
                        currentIDThing++;
                    }
                }
            }
        }

        public void SetGridAreas(Node startNode, int currentAreaID)
        {
            Heap<Node> openSet = new Heap<Node>(Maxsize);
            Heap<Node> closedList = new Heap<Node>(Maxsize);

            openSet.Add(startNode);

            Node neighbour;
            Node currentNode;

            while (openSet.Count > 0)
            {
                currentNode = openSet.RemoveFirst();
                closedList.Add(currentNode);

                for (int i = 0; i < currentNode.neighbours.Length; i++)
                {
                    neighbour = currentNode.neighbours[i];

                    if (neighbour == null || neighbour.walkable == NodeType.obstacle || closedList.Contains(neighbour))
                    {
                        continue;
                    }

                    if (openSet.Contains(neighbour) == false)
                    {
                        neighbour.gridAreaID = currentAreaID;
                        openSet.Add(neighbour);
                    }
                }
            }

            print("Number of nodes:" + closedList.Count + ". Number of grid nodes:" + Maxsize);
        }


        public void ResetNodes()
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    grid[x, y].inClosedList = false;
                    grid[x, y].inOpenSet = false;
                }
            }
        }

        public void GetNeighbours(Node node)
        {
            //Node[] neighbours = new Node[8];

            Node newNode;

            int checkX;
            int checkY;
            int index = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (connectionsOptions.Equals(Connections.directional4) && (Mathf.Abs(x) + Mathf.Abs(y) == 2))
                    {
                        continue;
                    }

                    //Skip center node, because it is current node
                    if (x == 0 && y == 0)
                        continue;

                    checkX = node.gridX + x;
                    checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        newNode = grid[checkX, checkY];

                        if (node.parent == newNode)
                        {
                            continue;
                        }
                        //Calculate obstacles while creating path
                        //AStar.CheckIfNodeIsObstacle(newNode);

                        //Prevent corner cutting
                        if (connectionsOptions.Equals(Connections.directional8DontCutCorners) &&
                            (grid[checkX, checkY].walkable == NodeType.obstacle ||
                             grid[checkX, node.gridY].walkable == NodeType.obstacle ||
                             grid[node.gridX, checkY].walkable == NodeType.obstacle))
                        {
                            continue;
                        }
                        else
                        {
                            node.neighbours[index] = newNode;
                            index++;
                        }
                    }
                }
            }
            //return neighbours;
        }


        public Node NodeFromWorldPoint(Vector2 worldPosition)
        {
            float positionOfNodeInGridX = (worldPosition.x - transform.position.x);
            float positionOfNodeInGridY = (worldPosition.y - transform.position.y);
            float percentX = (positionOfNodeInGridX + gridWorldSize.x / 2) / gridWorldSize.x;
            float percentY = (positionOfNodeInGridY + gridWorldSize.y / 2) / gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
            return grid[x, y];
        }


        public Node ClosestNodeFromWorldPoint(Vector2 worldPosition, int nodeArea)
        {
            float positionOfNodeInGridX = (worldPosition.x - transform.position.x);
            float positionOfNodeInGridY = (worldPosition.y - transform.position.y);
            float percentX = (positionOfNodeInGridX + gridWorldSize.x / 2) / gridWorldSize.x;
            float percentY = (positionOfNodeInGridY + gridWorldSize.y / 2) / gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
            //If target node is inside collider return nearby node
            if (grid[x, y].walkable == NodeType.obstacle || grid[x, y].gridAreaID != nodeArea)
            {
                //Node[] neighbours 
                Node neighbour = FindWalkableInRadius(x, y, 1, nodeArea);
                if (neighbour != null)
                {
                    return neighbour;
                }
            }

            return grid[x, y];
        }

        public static void CheckIfNodeIsObstacle(Node node)
        {
            ////Calculate obstacles while creating path
            Collider2D[] colliders = Physics2D.OverlapCircleAll(node.worldPosition,
                Instance.nodeRadius * Instance.collisionRadius, Instance.unwalkableMask);
            if (colliders.Length > 0)
            {
                node.walkable = NodeType.obstacle;
            }
            else
            {
                node.walkable = NodeType.walkable;
            }
        }

        private Node FindWalkableInRadius(int centreX, int centreY, int radius, int nodeArea)
        {
            if (radius > nearestNodeDistance)
            {
                UnityEngine.Debug.LogWarning("Target area is not in nearestNodeDistance!");
                return null;
            }

            for (int i = -radius; i <= radius; i++)
            {
                int verticalSearchX = i + centreX;
                int horizontalSearchY = i + centreY;

                // top
                if (InBounds(verticalSearchX, centreY + radius))
                {
                    if (grid[verticalSearchX, centreY + radius].walkable == NodeType.walkable &&
                        grid[verticalSearchX, centreY + radius].gridAreaID == nodeArea)
                    {
                        return grid[verticalSearchX, centreY + radius];
                    }
                }

                // bottom
                if (InBounds(verticalSearchX, centreY - radius))
                {
                    if (grid[verticalSearchX, centreY - radius].walkable == NodeType.walkable &&
                        grid[verticalSearchX, centreY - radius].gridAreaID == nodeArea)
                    {
                        return grid[verticalSearchX, centreY - radius];
                    }
                }

                // right
                if (InBounds(centreX + radius, horizontalSearchY))
                {
                    if (grid[centreX + radius, horizontalSearchY].walkable == NodeType.walkable &&
                        grid[centreX + radius, horizontalSearchY].gridAreaID == nodeArea)
                    {
                        return grid[centreX + radius, horizontalSearchY];
                    }
                }

                // left
                if (InBounds(centreX - radius, horizontalSearchY))
                {
                    if (grid[centreX - radius, horizontalSearchY].walkable == NodeType.walkable &&
                        grid[centreX - radius, horizontalSearchY].gridAreaID == nodeArea)
                    {
                        return grid[centreX - radius, horizontalSearchY];
                    }
                }
            }

            radius++;
            return FindWalkableInRadius(centreX, centreY, radius, nodeArea);
        }

        private bool InBounds(int x, int y)
        {
            return x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY;
        }


        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

            if (showGrid)
            {
                if (grid != null)
                {
                    foreach (Node n in grid)
                    {
                        Gizmos.color = (n.walkable == NodeType.walkable)
                            ? new Color(255, 255, 255, 0.4f)
                            : new Color(255, 0, 0, 0.4f);
                        //if (path != null)
                        //    if (path.Contains(n))
                        //        Gizmos.color = Color.black;
                        Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter - .1f));
                    }
                }

                if (pathFound)
                {
                    //Shows nodes added to open list
                    Gizmos.color = Color.yellow;
                    for (int i = 0; i < openList.Count; i++)
                    {
                        Gizmos.DrawSphere(openList[i].worldPosition, (nodeRadius - .1f));
                    }

                    //Shows nodes added to closed list
                    Gizmos.color = Color.red;
                    for (int i = 0; i < closedList.Count; i++)
                    {
                        Gizmos.DrawCube(closedList[i].worldPosition, Vector2.one * (nodeDiameter - .1f) * 0.3f);
                    }

                    //Draws line from node to it's parent
                    Gizmos.color = Color.green;
                    for (int i = 0; i < closedList.Count; i++)
                    {
                        if (closedList[i].parent != null)
                        {
                            Gizmos.DrawLine(closedList[i].worldPosition, closedList[i].parent.worldPosition);
                        }
                    }
                }
            }
        }
    }
}
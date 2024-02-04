using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding
{
    public static List<Vector2> FindPath(Vector2 start, Vector2 target, LayerMask obstacleLayer)
    {
        Node startNode = new Node(GridFromWorldPoint(start));
        Node targetNode = new Node(GridFromWorldPoint(target));

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                // Path found
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode, obstacleLayer))
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null; // No path found
    }

    static List<Vector2> RetracePath(Node startNode, Node endNode)
    {
        List<Vector2> path = new List<Vector2>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.worldPosition);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    static List<Node> GetNeighbors(Node node, LayerMask obstacleLayer)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                Vector2 neighborPosition = node.worldPosition + new Vector2(x, y);
                Node neighbor = new Node(GridFromWorldPoint(neighborPosition));

                // Check if the neighbor is within the obstacle layer
                if (Physics2D.OverlapCircle(neighbor.worldPosition, 0.2f, obstacleLayer) == null)
                {
                    neighbors.Add(neighbor);
                }
            }
        }

        return neighbors;
    }

    static int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return distX + distY;
    }

    static Vector2 GridFromWorldPoint(Vector2 worldPosition)
    {
        return new Vector2(Mathf.FloorToInt(worldPosition.x), Mathf.FloorToInt(worldPosition.y));
    }

    class Node
    {
        public int gridX, gridY;
        public Vector2 worldPosition;
        public int gCost, hCost;
        public Node parent;

        public Node(Vector2 _worldPos)
        {
            worldPosition = _worldPos;
            gridX = Mathf.FloorToInt(worldPosition.x);
            gridY = Mathf.FloorToInt(worldPosition.y);
        }

        public int fCost { get { return gCost + hCost; } }
    }
}
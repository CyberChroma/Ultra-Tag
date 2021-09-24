using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestDistance
{
    public PathfindingObject nextStep;
    public float distance;
}

public class PathfindingObject : MonoBehaviour
{
    public List<PathfindingObject> connectedObjects = new List<PathfindingObject>();
    public PathfindingObject farthestObject;
    public Dictionary<PathfindingObject, ShortestDistance> shortestPath = new Dictionary<PathfindingObject, ShortestDistance>();
}

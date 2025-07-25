using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> connectedObjects = new List<Waypoint>();
    public int[] shortestPathNextSteps;
    public Waypoint farthestObject;

    [HideInInspector] public bool isAirWaypoint = false;
}
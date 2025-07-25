using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClosestPathfinding : MonoBehaviour
{
    public List<Waypoint> closeRangeWaypoints;
    public Waypoint closestWaypoint;

    private ITCharacterTracker itCharacterTracker;
    private PathCalculator pathCalculator;

    // Start is called before the first frame update
    void Start()
    {
        closeRangeWaypoints = new List<Waypoint>();
        itCharacterTracker = FindFirstObjectByType<ITCharacterTracker>();
        pathCalculator = FindFirstObjectByType<PathCalculator>();
        closestWaypoint = GetClosestWaypoint();
    }

    void Update()
    {
        if (pathCalculator != null) {
            closestWaypoint = GetClosestWaypoint();
        }
    }

    public Waypoint GetClosestWaypoint()
    {
        Waypoint[] arrayToUse = new Waypoint[0];
        if (closeRangeWaypoints.Count != 0) {
            arrayToUse = closeRangeWaypoints.ToArray();
        }
        else {
            arrayToUse = pathCalculator.waypoints;
        }
        if (arrayToUse.Length != 0) {
            float minDis = Mathf.Infinity;
            int minDisIndex = 0;
            float currDis = 0;
            for (int i = 0; i < arrayToUse.Length; i++) {
                currDis = (arrayToUse[i].transform.position - transform.position).magnitude;
                if (currDis < minDis) {
                    minDis = currDis;
                    minDisIndex = i;
                }
            }

            Waypoint closestToSelf = arrayToUse[minDisIndex];
            return closestToSelf;
        } else {
            return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Waypoint newCloseObject = other.GetComponent<Waypoint>();
        if (newCloseObject != null) {
            closeRangeWaypoints.Add(newCloseObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Waypoint newCloseObject = other.GetComponent<Waypoint>();
        if (newCloseObject != null) {
            if (closeRangeWaypoints.Contains(newCloseObject)) {
                closeRangeWaypoints.Remove(newCloseObject);
            } else {
                print("THIS CODE SHOULD NOT RUN!!!");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClosestPathfinding : MonoBehaviour
{
    public List<PathfindingObject> closeRangePathfindingObjects;
    public PathfindingObject closestPathfindingObject;

    private ITCharacterTracker itCharacterTracker;
    private PathCalculator pathCalculator;

    // Start is called before the first frame update
    void Start()
    {
        closeRangePathfindingObjects = new List<PathfindingObject>();
        itCharacterTracker = FindObjectOfType<ITCharacterTracker>();
        pathCalculator = FindObjectOfType<PathCalculator>();
        closestPathfindingObject = GetClosestPathfindingObject();
    }

    void Update()
    {
        if (pathCalculator != null) {
            closestPathfindingObject = GetClosestPathfindingObject();
            if (itCharacterTracker.ITCharacter == transform.parent) {
                pathCalculator.closestToIT = closestPathfindingObject;
            }
        }
    }

    public PathfindingObject GetClosestPathfindingObject()
    {
        PathfindingObject[] arrayToUse;
        if (closeRangePathfindingObjects.Count != 0) {
            arrayToUse = closeRangePathfindingObjects.ToArray();
        }
        else {
            arrayToUse = pathCalculator.pathfindingObjects;
        }
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

        PathfindingObject closestToSelf = arrayToUse[minDisIndex];
        /*if (itCharacterTracker.ITCharacter != transform.parent && closestToSelf == pathCalculator.closestToIT && closestToSelf.connectedObjects.Count != 0) {
            List<PathfindingObject> connections = closestToSelf.connectedObjects;
            float thisCharacterDis;
            float itCharacterDis;
            float varianceOfDis;
            float maxDis = 0;
            int maxDisIndex = 0;
            for (int i = 0; i < connections.Count; i++) {
                if (connections[i] != null) {
                    thisCharacterDis = (connections[i].transform.position - transform.position).magnitude;
                    itCharacterDis = (connections[i].transform.position - itCharacterTracker.ITCharacter.position).magnitude;
                    varianceOfDis = itCharacterDis - thisCharacterDis;
                    if (varianceOfDis > maxDis) {
                        maxDis = currDis;
                        maxDisIndex = i;
                    }
                }
            }

            return connections[maxDisIndex];
        } else {*/
        return closestToSelf;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        PathfindingObject newCloseObject = other.GetComponent<PathfindingObject>();
        if (newCloseObject != null) {
            closeRangePathfindingObjects.Add(newCloseObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PathfindingObject newCloseObject = other.GetComponent<PathfindingObject>();
        if (newCloseObject != null) {
            if (closeRangePathfindingObjects.Contains(newCloseObject)) {
                closeRangePathfindingObjects.Remove(newCloseObject);
            } else {
                print("THIS CODE SHOULD NOT RUN!!!");
            }
        }
    }
}

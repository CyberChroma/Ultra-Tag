using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class PathfindingEditorTools : MonoBehaviour
{
    public bool drawConnections;
    public Waypoint waypointGroundPrefab;
    public Waypoint waypointAirPrefab;
    public float jumpHeight = 2;
    public float jumpDistance = 8;
    public float standHeight = 2;
    public float density;
    public Vector3 mapSize;
    public LayerMask waypointLayer;
    public LayerMask waypointAndEnvironmentLayers;
    public LayerMask waypointEnvironmentAndBlockersLayers;

    public List<Waypoint> waypoints = new List<Waypoint>();

    public void GenerateWaypoints()
    {
        if (density != 0) {
            List<Waypoint> airWaypoints = new List<Waypoint>();
            int waypointNum = 0;
            waypoints = new List<Waypoint>(FindObjectsByType<Waypoint>(FindObjectsSortMode.InstanceID));
            foreach (Waypoint waypoint in waypoints) {
                if (waypoint) {
                    DestroyImmediate(waypoint.gameObject);
                }
            }
            waypoints.Clear();
            Vector3 currPosition;
            for (float y = -1; y < mapSize.y; y += density) {
                for (float x = -mapSize.x; x < mapSize.x; x += density) {
                    for (float z = -mapSize.z; z < mapSize.z; z += density) {
                        currPosition = new Vector3(x, y, z);
                        if (!Physics.CheckSphere(currPosition, 0.001f, waypointEnvironmentAndBlockersLayers.value)) { // If this point is not inside a wall
                            RaycastHit hitInfo;
                            if (Physics.Raycast(currPosition, Vector3.down, out hitInfo, mapSize.y, waypointEnvironmentAndBlockersLayers.value, QueryTriggerInteraction.Collide)) {
                                Waypoint hitBelowWaypoint = hitInfo.collider.GetComponent<Waypoint>();
                                if (hitBelowWaypoint) {
                                    Waypoint hitWaypoint = null;
                                    bool canSpawn = false;
                                    if (Physics.Linecast(currPosition, new Vector3(x - density, y, z - density), out hitInfo, waypointEnvironmentAndBlockersLayers.value, QueryTriggerInteraction.Collide)) {
                                        hitWaypoint = hitInfo.collider.GetComponent<Waypoint>();
                                        if (hitWaypoint && !hitWaypoint.isAirWaypoint) {
                                            canSpawn = true;
                                        }
                                    }
                                    if (!canSpawn && Physics.Linecast(currPosition, new Vector3(x - density, y, z), out hitInfo, waypointEnvironmentAndBlockersLayers.value, QueryTriggerInteraction.Collide)) {
                                        hitWaypoint = hitInfo.collider.GetComponent<Waypoint>();
                                        if (hitWaypoint && !hitWaypoint.isAirWaypoint) {
                                            canSpawn = true;
                                        }
                                    }
                                    if (!canSpawn && Physics.Linecast(currPosition, new Vector3(x - density, y, z + density), out hitInfo, waypointEnvironmentAndBlockersLayers.value, QueryTriggerInteraction.Collide)) {
                                        hitWaypoint = hitInfo.collider.GetComponent<Waypoint>();
                                        if (hitWaypoint && !hitWaypoint.isAirWaypoint) {
                                            canSpawn = true;
                                        }
                                    }
                                    if (!canSpawn && Physics.Linecast(currPosition, new Vector3(x, y, z - density), out hitInfo, waypointEnvironmentAndBlockersLayers.value, QueryTriggerInteraction.Collide)) {
                                        hitWaypoint = hitInfo.collider.GetComponent<Waypoint>();
                                        if (hitWaypoint && !hitWaypoint.isAirWaypoint) {
                                            canSpawn = true;
                                        }
                                    }
                                    if (canSpawn) {
                                        if (hitBelowWaypoint.transform.position.y > y - density || (hitBelowWaypoint.transform.position.y <= y - density && !hitWaypoint.connectedObjects.Contains(hitBelowWaypoint))) {
                                            Waypoint newWaypoint = Instantiate(waypointAirPrefab, currPosition, Quaternion.identity, transform).GetComponent<Waypoint>();
                                            AirConnections(newWaypoint, hitBelowWaypoint, waypointNum, currPosition, airWaypoints);
                                            waypointNum++;
                                        }
                                    }
                                }
                                else if (hitInfo.collider.gameObject.layer != 11 && !Physics.Raycast(hitInfo.point, Vector3.up, standHeight, waypointAndEnvironmentLayers.value)) {
                                    Waypoint newWaypoint = Instantiate(waypointGroundPrefab, currPosition, Quaternion.identity, transform).GetComponent<Waypoint>();
                                    newWaypoint.name = "Waypoint " + waypointNum + " " + currPosition;
                                    waypoints.Add(newWaypoint);
                                    waypointNum++;
                                    for (float densy = -density; densy <= density; densy += density) {
                                        for (float densx = -density; densx <= density; densx += density) {
                                            for (float densz = -density; densz <= density; densz += density) {
                                                if (densx == 0 && densy == -density && densz == 0) {
                                                    continue;
                                                }
                                                else if (densx == 0 && densy == 0 && densz == 0) {
                                                    break;
                                                }
                                                if (Physics.Linecast(currPosition, new Vector3(x + densx, y + densy, z + densz), out hitInfo, waypointEnvironmentAndBlockersLayers.value, QueryTriggerInteraction.Collide)) {
                                                    Waypoint hitWaypoint = hitInfo.collider.GetComponent<Waypoint>();
                                                    if (hitWaypoint) {
                                                        newWaypoint.connectedObjects.Add(hitWaypoint);
                                                        hitWaypoint.connectedObjects.Add(newWaypoint);
                                                    }
                                                }
                                                else if (densy == 0) {
                                                    Vector3 airPosition = new Vector3(x + densx, y, z + densz);
                                                    if (!Physics.CheckSphere(airPosition, 0.001f, waypointEnvironmentAndBlockersLayers.value)) {
                                                        if (Physics.Raycast(airPosition, Vector3.down, out hitInfo, mapSize.y, waypointEnvironmentAndBlockersLayers.value, QueryTriggerInteraction.Collide)) {
                                                            Waypoint hitBelowAirWaypoint = hitInfo.collider.GetComponent<Waypoint>();
                                                            if (hitBelowAirWaypoint) {
                                                                if (hitBelowAirWaypoint.transform.position.y > y - density || (hitBelowAirWaypoint.transform.position.y <= y - density && !newWaypoint.connectedObjects.Contains(hitBelowAirWaypoint))) {
                                                                    Waypoint newAirWaypoint = Instantiate(waypointAirPrefab, airPosition, Quaternion.identity, transform).GetComponent<Waypoint>();
                                                                    newWaypoint.connectedObjects.Add(newAirWaypoint);
                                                                    newAirWaypoint.connectedObjects.Add(newWaypoint);
                                                                    AirConnections(newAirWaypoint, hitBelowAirWaypoint, waypointNum, airPosition, airWaypoints);
                                                                    waypointNum++;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (densx == 0 && densy == 0) {
                                                break;
                                            }
                                        }
                                        if (densy == 0) {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void AirConnections(Waypoint waypoint, Waypoint belowWaypoint, int waypointNum, Vector3 currPosition, List<Waypoint> airWaypoints)
    {
        waypoint.name = "Waypoint " + waypointNum + " " + currPosition;
        waypoints.Add(waypoint);
        waypoint.connectedObjects.Add(belowWaypoint);
        if (Mathf.Abs(currPosition.y - belowWaypoint.transform.position.y) <= jumpHeight) {
            belowWaypoint.connectedObjects.Add(waypoint);
        }

        for (float densy = -density; densy <= density; densy += density) {
            for (float densx = -density; densx <= density; densx += density) {
                for (float densz = -density; densz <= density; densz += density) {
                    if (densx == 0 && densy == -density && densz == 0) {
                        continue;
                    }
                    else if (densx == 0 && densy == 0 && densz == 0) {
                        break;
                    }
                    RaycastHit hitInfo;
                    if (Physics.Linecast(currPosition, new Vector3(currPosition.x + densx, currPosition.y + densy, currPosition.z + densz), out hitInfo, waypointEnvironmentAndBlockersLayers.value, QueryTriggerInteraction.Collide)) {
                        Waypoint hitWaypoint = hitInfo.collider.GetComponent<Waypoint>();
                        if (hitWaypoint && !hitWaypoint.isAirWaypoint) {
                            waypoint.connectedObjects.Add(hitWaypoint);
                            hitWaypoint.connectedObjects.Add(waypoint);
                        }
                    }
                }
                if (densx == 0 && densy == 0) {
                    break;
                }
            }
            if (densy == 0) {
                break;
            }
        }

        for (int i = 0; i < airWaypoints.Count; i++) {
            RaycastHit hitInfo;
            Transform currAirWaypoint = airWaypoints[i].transform;
            if (Mathf.Abs(currAirWaypoint.position.x - waypoint.transform.position.x) > density && Mathf.Abs(currAirWaypoint.position.z - waypoint.transform.position.z) > density && Physics.Linecast(currPosition, currAirWaypoint.position, out hitInfo, waypointEnvironmentAndBlockersLayers.value, QueryTriggerInteraction.Collide) && hitInfo.transform == currAirWaypoint) {
                float horizontalDis = Vector3.Distance(new Vector3(currPosition.x, 0, currPosition.z), new Vector3(currAirWaypoint.position.x, 0, currAirWaypoint.position.z));
                float verticalDis = currPosition.y - currAirWaypoint.position.y;
                if (horizontalDis <= jumpDistance) {
                    if (verticalDis < -jumpHeight) {
                        airWaypoints[i].connectedObjects.Add(waypoint);
                    }
                    else if (verticalDis > jumpHeight) {
                        waypoint.connectedObjects.Add(airWaypoints[i]);
                    }
                    else {
                        waypoint.connectedObjects.Add(airWaypoints[i]);
                        airWaypoints[i].connectedObjects.Add(waypoint);
                    }
                }
            }
        }
        airWaypoints.Add(waypoint);
    }

    public void ReduceWaypoints ()
    {
        for(int i = 0; i < waypoints.Count; i++) {
            Waypoint currWaypoint = waypoints[i];
            if (currWaypoint && !currWaypoint.isAirWaypoint && currWaypoint.connectedObjects.Count == 8) {
                bool allGround = true;
                for (int j = 0; j < currWaypoint.connectedObjects.Count; j++) {
                    if (currWaypoint.connectedObjects[j].isAirWaypoint) {
                        allGround = false;
                    }
                }
                if (allGround) {
                    bool surrounded = true;
                    for (float x = -density; x <= density; x += density) {
                        for (float z = -density; z <= density; z += density) {
                            if ((x != 0 || z != 0) && !Physics.CheckSphere(new Vector3(currWaypoint.transform.position.x + x, currWaypoint.transform.position.y, currWaypoint.transform.position.z + z), 0.001f, waypointLayer.value)) {
                                surrounded = false;
                            }
                        }
                    }
                    if (surrounded) {
                        List<Waypoint> surroundingWaypoints = new List<Waypoint>(currWaypoint.connectedObjects);
                        for (int j = 0; j < waypoints.Count; j++) {
                            Waypoint possibleConnectionWaypoint = waypoints[j];
                            if (surroundingWaypoints.Contains(possibleConnectionWaypoint)) {
                                for (int k = 0; k < possibleConnectionWaypoint.connectedObjects.Count; k++) {
                                    Waypoint possibleConnectionOutsideNineSlice = possibleConnectionWaypoint.connectedObjects[k];
                                    if (possibleConnectionOutsideNineSlice != currWaypoint && !currWaypoint.connectedObjects.Contains(possibleConnectionOutsideNineSlice)) {
                                        currWaypoint.connectedObjects.Add(possibleConnectionOutsideNineSlice);
                                    }
                                }
                            }
                            else if (possibleConnectionWaypoint != currWaypoint) {
                                for (int k = 0; k < possibleConnectionWaypoint.connectedObjects.Count; k++) {
                                    Waypoint possibleConnectionInsideNineSlice = possibleConnectionWaypoint.connectedObjects[k];
                                    if (surroundingWaypoints.Contains(possibleConnectionInsideNineSlice)) {
                                        possibleConnectionWaypoint.connectedObjects.Remove(possibleConnectionInsideNineSlice);
                                        if (!possibleConnectionWaypoint.connectedObjects.Contains(currWaypoint)) {
                                            possibleConnectionWaypoint.connectedObjects.Add(currWaypoint);
                                        }
                                    }
                                }
                            }
                        }

                        for (int j = 0; j < surroundingWaypoints.Count; j++) {
                            currWaypoint.connectedObjects.Remove(surroundingWaypoints[j]);
                            waypoints.Remove(surroundingWaypoints[j]);
                            DestroyImmediate(surroundingWaypoints[j].gameObject);
                        }
                    }
                }
            }
        }
    }

    public void CleanUp()
    {
        foreach (Waypoint waypoint in waypoints) {
            if (waypoint != null) {
                List<Waypoint> connectionsToRemove = new List<Waypoint>();
                foreach (Waypoint connection in waypoint.connectedObjects) {
                    if (connection == null) {
                        connectionsToRemove.Add(connection);
                    }
                }

                foreach (Waypoint connectionToRemove in connectionsToRemove) {
                    waypoint.connectedObjects.Remove(connectionToRemove);
                }
            }
        }
        Rename();
    }

    private void Rename()
    {
        int currName = 0;
        foreach (Waypoint waypoint in waypoints) {
            if (waypoint != null) {
                waypoint.name = "Waypoint " + currName + " " + waypoint.transform.position;
                currName++;
            }
        }
    }
}

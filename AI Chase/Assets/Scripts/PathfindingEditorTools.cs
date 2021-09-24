using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PathfindingEditorTools : MonoBehaviour
{
    public bool drawConnections;
    public PathfindingObject pathfindingObjectPrefab;

    [HideInInspector] public PathfindingObject[] pathfindingObjects;

    public void DrawConnections()
    {
        List<PathfindingObject> connections = new List<PathfindingObject>();
        foreach (PathfindingObject pathfindingObject in pathfindingObjects) {
            if (pathfindingObject != null) {
                connections = pathfindingObject.connectedObjects;
                bool found;
                foreach (PathfindingObject connection in connections) {
                    if (connection != null) {
                        found = false;
                        foreach (PathfindingObject otherConnection in connection.connectedObjects) {
                            if (pathfindingObject == otherConnection) {
                                found = true;
                                break;
                            }
                        }
                        if (found) {
                            Handles.color = Color.blue;
                            Handles.DrawLine(pathfindingObject.transform.position, connection.transform.position);
                        }
                        else {
                            Handles.color = Color.red;
                            Handles.DrawLine(pathfindingObject.transform.position, connection.transform.position);
                        }
                    }
                }
            }
        }
    }

    public void MakeNewConnection(PathfindingObject[] selectedPathfindingObjects)
    {
        Vector3 positionSum = Vector3.zero;
        foreach (PathfindingObject selection in selectedPathfindingObjects) {
            positionSum += selection.transform.position;
        }
        Vector3 averagePos = positionSum / selectedPathfindingObjects.Length;
        GameObject newPathfindingObjectPrefab = (GameObject)PrefabUtility.InstantiatePrefab(PrefabUtility.GetCorrespondingObjectFromSource(selectedPathfindingObjects[0].gameObject), transform);
        newPathfindingObjectPrefab.transform.position = averagePos;

        PathfindingObject newPathfindingObject = newPathfindingObjectPrefab.GetComponent<PathfindingObject>();

        foreach (PathfindingObject currSelection in selectedPathfindingObjects) {
            List<PathfindingObject> connectionsToRemove = new List<PathfindingObject>();
            foreach (PathfindingObject connection in currSelection.connectedObjects) {
                foreach (PathfindingObject otherSelection in selectedPathfindingObjects) {
                    if (currSelection != otherSelection && connection == otherSelection) {
                        connectionsToRemove.Add(otherSelection);
                    }
                }
            }

            foreach (PathfindingObject connectionToRemove in connectionsToRemove) {
                currSelection.connectedObjects.Remove(connectionToRemove);
            }

            currSelection.connectedObjects.Add(newPathfindingObject);
            newPathfindingObject.connectedObjects.Add(currSelection);
            EditorUtility.SetDirty(currSelection);
        }
        EditorUtility.SetDirty(newPathfindingObjectPrefab);
    }

    public void MakeConnections(PathfindingObject[] selectedPathfindingObjects)
    {
        foreach (PathfindingObject currSelection in selectedPathfindingObjects) {
            foreach (PathfindingObject otherSelection in selectedPathfindingObjects) {
                if (currSelection != otherSelection) {
                    if (!currSelection.connectedObjects.Contains(otherSelection)) {
                        currSelection.connectedObjects.Add(otherSelection);
                    }
                }
            }
            EditorUtility.SetDirty(currSelection);
        }
    }

    public void RemoveConnections(PathfindingObject[] selectedPathfindingObjects)
    {
        foreach (PathfindingObject currSelection in selectedPathfindingObjects) {
            foreach (PathfindingObject otherSelection in selectedPathfindingObjects) {
                if (currSelection != otherSelection) {
                    if (currSelection.connectedObjects.Contains(otherSelection)) {
                        currSelection.connectedObjects.Remove(otherSelection);
                    }
                }
            }
            EditorUtility.SetDirty(currSelection);
        }
    }

    public void CleanUp()
    {
        foreach (PathfindingObject pathfindingObject in pathfindingObjects) {
            List<PathfindingObject> connectionsToRemove = new List<PathfindingObject>();
            foreach (PathfindingObject connection in pathfindingObject.connectedObjects) {
                if (connection == null) {
                    connectionsToRemove.Add(connection);
                }
            }

            foreach (PathfindingObject connectionToRemove in connectionsToRemove) {
                pathfindingObject.connectedObjects.Remove(connectionToRemove);
            }
            EditorUtility.SetDirty(pathfindingObject);
        }
    }
}

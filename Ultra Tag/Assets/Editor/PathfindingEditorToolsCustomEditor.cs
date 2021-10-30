using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[ExecuteInEditMode]
[CustomEditor(typeof(PathfindingEditorTools))]
public class PathfindingEditorToolsCustomEditor : Editor
{
    private PathfindingEditorTools pathfindingEditorTools;
    private PathCalculator pathCalculator;
    private float density;
    private Vector3 mapSize;

    // Start is called before the first frame update
    void OnEnable()
    {
        pathfindingEditorTools = FindObjectOfType<PathfindingEditorTools>();
        pathCalculator = FindObjectOfType<PathCalculator>();
        SceneView.duringSceneGui += this.OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (pathfindingEditorTools != null &&pathfindingEditorTools.drawConnections) {
            DrawConnections();
        }
    }

    void DrawConnections()
    {
        List<Waypoint> connections = new List<Waypoint>();
        foreach (Waypoint waypoint in pathfindingEditorTools.waypoints) {
            if (waypoint != null) {
                connections = waypoint.connectedObjects;
                bool found;
                foreach (Waypoint connection in connections) {
                    if (connection != null) {
                        found = false;
                        foreach (Waypoint otherConnection in connection.connectedObjects) {
                            if (waypoint == otherConnection) {
                                found = true;
                                break;
                            }
                        }
                        if (found) {
                            Handles.color = Color.blue;
                            Handles.DrawLine(waypoint.transform.position, connection.transform.position);
                        }
                        else {
                            Handles.color = Color.red;
                            Handles.DrawLine(waypoint.transform.position, connection.transform.position);
                        }
                    }
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Waypoints")) {
            pathfindingEditorTools.GenerateWaypoints();
            for(int i = 0; i < pathfindingEditorTools.waypoints.Count; i++) {
                EditorUtility.SetDirty(pathfindingEditorTools.waypoints[i].gameObject);
            }
            EditorSceneManager.MarkSceneDirty(pathfindingEditorTools.gameObject.scene);
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Reduce Waypoints")) {
            pathfindingEditorTools.ReduceWaypoints();
            for (int i = 0; i < pathfindingEditorTools.waypoints.Count; i++) {
                EditorUtility.SetDirty(pathfindingEditorTools.waypoints[i].gameObject);
            }
            EditorSceneManager.MarkSceneDirty(pathfindingEditorTools.gameObject.scene);
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Calculate Paths")) {
            pathCalculator.CalculatePaths();
            for (int i = 0; i < pathfindingEditorTools.waypoints.Count; i++) {
                EditorUtility.SetDirty(pathfindingEditorTools.waypoints[i].gameObject);
            }
            EditorSceneManager.MarkSceneDirty(pathfindingEditorTools.gameObject.scene);
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Clean Up")) {
            pathfindingEditorTools.CleanUp();
            for (int i = 0; i < pathfindingEditorTools.waypoints.Count; i++) {
                EditorUtility.SetDirty(pathfindingEditorTools.waypoints[i].gameObject);
            }
            EditorSceneManager.MarkSceneDirty(pathfindingEditorTools.gameObject.scene);
        }
        EditorGUILayout.Space();
    }
}

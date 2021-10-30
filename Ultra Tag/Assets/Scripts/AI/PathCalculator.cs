using UnityEngine;

public class PathCalculator : MonoBehaviour
{
    [HideInInspector] public Waypoint[] waypoints;

    public float[,] distances;

    public void CalculatePaths()
    {
        waypoints = transform.GetComponentsInChildren<Waypoint>();
        distances = new float[waypoints.Length,waypoints.Length];

        for(int x = 0; x < waypoints.Length; x++) {
            waypoints[x].name = "Waypoint " + x + " " + waypoints[x].transform.position;
            waypoints[x].shortestPathNextSteps = new int[waypoints.Length];
            for (int y = 0; y < waypoints.Length; y++) {
                if (waypoints[x].connectedObjects.Contains(waypoints[y])) {
                    waypoints[x].shortestPathNextSteps[y] = waypoints[x].connectedObjects.IndexOf(waypoints[y]);
                    distances[x, y] = (waypoints[x].transform.position - waypoints[y].transform.position).magnitude;
                } else {
                    distances[x, y] = Mathf.Infinity;
                }
            }
            waypoints[x].shortestPathNextSteps[x] = waypoints[x].connectedObjects.IndexOf(waypoints[x]);
            distances[x, x] = 0;
        }

        for (int m = 0; m < waypoints.Length; m++) {
            for (int x = 0; x < waypoints.Length; x++) {
                for (int y = 0; y < waypoints.Length; y++) {
                    if (distances[x, m] + distances[m, y] < distances[x, y]) {
                        distances[x, y] = distances[x, m] + distances[m, y];
                        waypoints[x].shortestPathNextSteps[y] = waypoints[x].shortestPathNextSteps[m];
                    }
                }
            }
        }

        float maxDis;
        int maxDisIndex;
        for (int x = 0; x < waypoints.Length; x++) {
            maxDis = 0;
            maxDisIndex = 0;
            for (int i = 0; i < waypoints.Length; i++) {
                if (distances[x, i] > maxDis && distances[x, i] != Mathf.Infinity) {
                    maxDis = distances[x, i];
                    maxDisIndex = i;
                }
            }
            waypoints[x].farthestObject = waypoints[maxDisIndex];
        }
    } 
}

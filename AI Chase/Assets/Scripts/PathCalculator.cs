using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathCalculator : MonoBehaviour
{
    public PathfindingObject closestToIT;

    [HideInInspector] public PathfindingObject[] pathfindingObjects;

    private CharacterClosestPathfinding playerCharacterClosestPathfinding;


    // Start is called before the first frame update
    void Start()
    {
        pathfindingObjects = transform.GetComponentsInChildren<PathfindingObject>();
        playerCharacterClosestPathfinding = FindObjectOfType<PlayerInput>().GetComponentInChildren<CharacterClosestPathfinding>();
        CalculatePaths();
    }

    public void CalculatePaths()
    {
        if (Random.Range(0, 2) == 1) {
            pathfindingObjects.Reverse();
        }

        foreach (PathfindingObject x in pathfindingObjects) {
            foreach (PathfindingObject y in pathfindingObjects) {
                if (!x.shortestPath.ContainsKey(y)) {
                    x.shortestPath.Add(y, new ShortestDistance());
                }
                if (x.connectedObjects.Contains(y) && y != closestToIT) {
                    x.shortestPath[y].nextStep = y;
                    x.shortestPath[y].distance = (x.transform.position - y.transform.position).magnitude;
                } else {
                    x.shortestPath[y].distance = Mathf.Infinity;
                }
            }
            x.shortestPath[x].nextStep = x;
            x.shortestPath[x].distance = 0;
        }

        foreach (PathfindingObject m in pathfindingObjects) {
            foreach (PathfindingObject x in pathfindingObjects) {
                foreach (PathfindingObject y in pathfindingObjects) {
                    if (x.shortestPath[m].distance + m.shortestPath[y].distance < x.shortestPath[y].distance) {
                        x.shortestPath[y].distance = x.shortestPath[m].distance + m.shortestPath[y].distance;
                        x.shortestPath[y].nextStep = x.shortestPath[m].nextStep;
                    }
                }
            }
        }

        float maxDis;
        foreach (PathfindingObject x in pathfindingObjects) {
            maxDis = x.shortestPath.Values.Select(e => e.distance).Max();
            x.farthestObject = x.shortestPath.Where(e => e.Value.distance == maxDis).Random().Key;
        }
    }

}

public static class Randomize
{
    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        System.Random rand = new System.Random();
        int index = rand.Next(0, enumerable.Count());
        return enumerable.ElementAt(index);
    }
}

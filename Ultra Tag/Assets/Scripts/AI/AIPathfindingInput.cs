using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathfindingInput : MonoBehaviour
{
    public int difficulty;

    public float turnAmountToMoveForward = 90;

    public Waypoint currDestination;
    public Waypoint endDestination;

    private bool front;
    private bool back;
    private bool left;
    private bool right;

    private float amountToRotate;
    private float mouseHorizontal;

    private AITriggerInteractionInput aiTriggerInteractionInput;
    private CharacterMove characterMove;
    private CharacterTurn characterTurn;

    private Rigidbody rb;
    private ITCharacterTracker itCharacterTracker;
    private Transform lastITCharacter;
    private CharacterClosestPathfinding selfCharacterClosestPathfinding;
    private CharacterClosestPathfinding itCharacterClosestPathfinding;
    private CharacterClosestPathfinding closestCharacterClosestPathfinding;
    private CharacterClosestPathfinding[] allCharacterClosestPathfinding;
    private Waypoint selfClosestPath;
    private Waypoint itClosestPath;
    private Waypoint lastITClosestPath;
    private PathCalculator pathCalculator;

    // Start is called before the first frame update
    void Start()
    {
        aiTriggerInteractionInput = GetComponent<AITriggerInteractionInput>();
        characterMove = GetComponent<CharacterMove>();
        characterTurn = GetComponent<CharacterTurn>();
        rb = GetComponent<Rigidbody>();
        itCharacterTracker = FindObjectOfType<ITCharacterTracker>();
        selfCharacterClosestPathfinding = GetComponentInChildren<CharacterClosestPathfinding>();
        allCharacterClosestPathfinding = FindObjectsOfType<CharacterClosestPathfinding>();
        pathCalculator = FindObjectOfType<PathCalculator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetDestination();
        MoveInput();
        TurnInput();
    }

    void GetDestination()
    {
        if (itCharacterTracker.ITCharacters[0].transform != lastITCharacter) {
            ITCharacterChange();
        } else {
            itClosestPath = itCharacterClosestPathfinding.closestWaypoint;
        }
        selfClosestPath = selfCharacterClosestPathfinding.closestWaypoint;

        if (itClosestPath != null && itClosestPath != lastITClosestPath) {
            ITClosestWaypointChange();
        }
        if (selfClosestPath != null) {
            if (currDestination == endDestination && !itCharacterTracker.ITCharacters.Contains(transform)) {
                endDestination = pathCalculator.waypoints[Random.Range(0, pathCalculator.waypoints.Length)];
            }
            if (selfClosestPath.shortestPathNextSteps[int.Parse(endDestination.name.Split(' ')[1])] == -1) {
                NewEndDestination();
            } else {
                currDestination = selfClosestPath.connectedObjects[selfClosestPath.shortestPathNextSteps[int.Parse(endDestination.name.Split(' ')[1])]];
                if (currDestination.isAirWaypoint) {
                    StartCoroutine(aiTriggerInteractionInput.HoldJump(2));
                }
            }
        }

        lastITCharacter = itCharacterTracker.ITCharacters[0].transform;
        lastITClosestPath = itClosestPath;
    }

    void ITCharacterChange()
    {
        itCharacterClosestPathfinding = itCharacterTracker.ITCharacters[0].GetComponentInChildren<CharacterClosestPathfinding>();
        itClosestPath = itCharacterClosestPathfinding.closestWaypoint;
        NewEndDestination();
    }

    void ITClosestWaypointChange()
    {
        if (itCharacterTracker.ITCharacters.Contains(transform)) {
            float minDis = Mathf.Infinity;
            int minDisIndex = 0;
            float currDis = 0;
            for (int i = 0; i < allCharacterClosestPathfinding.Length; i++) {
                if (allCharacterClosestPathfinding[i] != selfCharacterClosestPathfinding && !itCharacterTracker.ITCharacters.Contains(allCharacterClosestPathfinding[i].transform.parent)) {
                    currDis = (allCharacterClosestPathfinding[i].transform.position - transform.position).magnitude;
                    if (currDis < minDis) {
                        minDis = currDis;
                        minDisIndex = i;
                    }
                }
            }
            closestCharacterClosestPathfinding = allCharacterClosestPathfinding[minDisIndex];
            endDestination = closestCharacterClosestPathfinding.closestWaypoint;
        }
        else {
            NewEndDestination();
        }
    }

    void NewEndDestination ()
    {
        if (Random.Range(0, difficulty) == 0) {
            endDestination = pathCalculator.waypoints[Random.Range(0, pathCalculator.waypoints.Length)];
        } else {
            endDestination = itClosestPath.farthestObject;
        }
    }

    void MoveInput()
    {
        if (Mathf.Abs(amountToRotate) > turnAmountToMoveForward) {
            front = false;
        } else {
            front = true;
        }
        back = false;
        left = false;
        right = false;
        characterMove.Move(front, back, left, right);
    }

    void TurnInput()
    {
        Vector3 closestCharacterPos;
        if (closestCharacterClosestPathfinding != null) {
            closestCharacterPos = closestCharacterClosestPathfinding.transform.position;
        } else {
            closestCharacterPos = Vector3.zero;
        }
        Vector3 objToFace = Vector3.zero;
        if (itCharacterTracker.ITCharacters.Contains(transform) && (Mathf.Abs(transform.position.y - closestCharacterPos.y) < 5) && (Vector3.Distance(transform.position, closestCharacterPos) < Vector3.Distance(transform.position, currDestination.transform.position) || currDestination == endDestination)) {
            objToFace = closestCharacterPos;
        } else if (currDestination != null) {
            objToFace = currDestination.transform.position;
        }
        float rotationToFace = Quaternion.LookRotation(objToFace - transform.position).eulerAngles.y;
        amountToRotate = transform.rotation.eulerAngles.y - rotationToFace;

        if (amountToRotate > 180) {
            amountToRotate -= 360;
        }
        else if (amountToRotate < -180) {
            amountToRotate += 360;
        }

        if (Mathf.Abs(amountToRotate) < 1) {
            mouseHorizontal = 0;
        } else if (amountToRotate > 0) {
            mouseHorizontal = -1;
        } else {
            mouseHorizontal = 1;
        }

        characterTurn.Turn(mouseHorizontal * Mathf.Abs(amountToRotate) / 100);
    }
}

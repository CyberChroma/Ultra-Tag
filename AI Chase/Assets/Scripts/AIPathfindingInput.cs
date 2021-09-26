using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathfindingInput : MonoBehaviour
{
    public int difficulty;

    public float turnAmountToMoveForward = 90;

    public PathfindingObject currDestination;
    public PathfindingObject endDestination;

    private bool front;
    private bool back;
    private bool left;
    private bool right;

    private float amountToRotate;
    private float mouseHorizontal;

    private CharacterMove characterMove;
    private CharacterTurn characterTurn;

    private Rigidbody rb;
    private ITCharacterTracker itCharacterTracker;
    private Transform lastITCharacter;
    private CharacterClosestPathfinding selfCharacterClosestPathfinding;
    private CharacterClosestPathfinding itCharacterClosestPathfinding;
    private CharacterClosestPathfinding closestCharacterClosestPathfinding;
    private CharacterClosestPathfinding[] allCharacterClosestPathfinding;
    private PathfindingObject selfClosestPath;
    private PathfindingObject itClosestPath;
    private PathfindingObject lastITClosestPath;
    private PathCalculator pathCalculator;

    // Start is called before the first frame update
    void Start()
    {
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
        if (itCharacterTracker.ITCharacter.transform != lastITCharacter) {
            ITCharacterChange();
        } else {
            itClosestPath = itCharacterClosestPathfinding.closestPathfindingObject;
        }
        selfClosestPath = selfCharacterClosestPathfinding.closestPathfindingObject;

        if (itClosestPath != null && itClosestPath != lastITClosestPath) {
            ITClosestWaypointChange();
        }
        if (selfClosestPath != null) {
            if (currDestination == endDestination && itCharacterTracker.ITCharacter != transform) {
                endDestination = pathCalculator.pathfindingObjects[Random.Range(0, pathCalculator.pathfindingObjects.Length)];
            }
            currDestination = selfClosestPath.shortestPath[endDestination].nextStep;
        }

        lastITCharacter = itCharacterTracker.ITCharacter.transform;
        lastITClosestPath = itClosestPath;
    }

    void ITCharacterChange()
    {
        itCharacterClosestPathfinding = itCharacterTracker.ITCharacter.GetComponentInChildren<CharacterClosestPathfinding>();
        itClosestPath = itCharacterClosestPathfinding.closestPathfindingObject;
        NewEndDestination();
    }

    void ITClosestWaypointChange()
    {
        if (itCharacterTracker.ITCharacter == transform) {
            float minDis = Mathf.Infinity;
            int minDisIndex = 0;
            float currDis = 0;
            for (int i = 0; i < allCharacterClosestPathfinding.Length; i++) {
                if (allCharacterClosestPathfinding[i] != selfCharacterClosestPathfinding) {
                    currDis = (allCharacterClosestPathfinding[i].transform.position - transform.position).magnitude;
                    if (currDis < minDis) {
                        minDis = currDis;
                        minDisIndex = i;
                    }
                }
            }
            closestCharacterClosestPathfinding = allCharacterClosestPathfinding[minDisIndex];
            endDestination = closestCharacterClosestPathfinding.closestPathfindingObject;
        }
        else {
            NewEndDestination();
        }
    }

    void NewEndDestination ()
    {
        if (Random.Range(0, difficulty) == 0) {
            endDestination = pathCalculator.pathfindingObjects[Random.Range(0, pathCalculator.pathfindingObjects.Length)];
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
        if (itCharacterTracker.ITCharacter == transform && (Mathf.Abs(transform.position.y - closestCharacterPos.y) < 5) && (Vector3.Distance(transform.position, closestCharacterPos) < Vector3.Distance(transform.position, currDestination.transform.position) || currDestination == endDestination)) {
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

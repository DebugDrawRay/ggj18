using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class AIController : MonoBehaviour, IInputController
{
    public ActionSet Actions
    {
        get;
        set;
    }
    public float repathRate = .5f;
    public float lastRepath;

    private Seeker seeker;
    private Path path;
    private int currentWaypoint;

    public float nextWaypointDistance = 2;
    public float playerTriggerDistance = 5f;

    public float throwDelay;
    private float currentDelay;
    private enum State
    {
        Idle,
        Moving,
        Throwing
    }
    private State currentState;

    private void Awake()
    {
        Actions = new ActionSet();
        seeker = GetComponent<Seeker>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                if (Vector3.Distance(transform.position, PlayerController.position) <= playerTriggerDistance)
                {
                    ChangeState(State.Moving);
                }
                break;
            case State.Moving:
                UpdateMovement();
                break;
            case State.Throwing:
                UpdateThrowing();
                break;
        }
    }

    private void ChangeState(State newState)
    {
        //Exit
        switch (currentState)
        {
            case State.Idle:
                break;
            case State.Moving:
                break;
            case State.Throwing:
                break;
        }
        //Enter
        switch (newState)
        {
            case State.Idle:
                break;
            case State.Moving:
                seeker.StartPath(transform.position, PlayerController.position, OnPathComplete);
                break;
            case State.Throwing:
                currentDelay = Time.time + throwDelay;
                break;
        }
        currentState = newState;
    }

    private void UpdateMovement()
    {
        if (path == null || currentWaypoint > path.vectorPath.Count)
        {
            Actions.moveVector = Vector3.zero;
            ChangeState(State.Throwing);
            return;
        }

        if (currentWaypoint == path.vectorPath.Count)
        {
            Actions.moveVector = Vector3.zero;
            ChangeState(State.Throwing);
            return;
        }
        Actions.moveVector = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        if ((transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance * nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

    private void UpdateThrowing()
    {
        GetComponent<StatusController>().CurrentFacing = (PlayerController.position - transform.position).normalized;
        GetComponent<Animator>().SetFloat("x", GetComponent<StatusController>().CurrentFacing.x);
        GetComponent<Animator>().SetFloat("y", GetComponent<StatusController>().CurrentFacing.z);
        Actions.primaryAction = true;
        if (Time.time >= currentDelay)
        {
            Actions.primaryAction = false;
            ChangeState(State.Moving);
        }
    }
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }

    }
}

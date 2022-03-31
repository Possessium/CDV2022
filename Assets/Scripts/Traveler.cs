using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Traveler : MonoBehaviour
{
    private NavMeshAgent agent;
    private TravelerZone reachableBounds;
    [SerializeField, Tooltip("Distance when the agent consider its point reached")] private float distanceReachedPoint = 1;
    [SerializeField] private int scoreValue = 10;
    public int ScoreValue { get { return scoreValue; } }
    private WindowDepot window;

    #region Gizmos
    private bool isRightSide;
    private Vector3 agentDestination;

    private void OnDrawGizmos()
    {
        Gizmos.color = isRightSide ? Color.green : Color.red;
        Gizmos.DrawSphere(agentDestination, .2f);
    }
    #endregion

    private void Awake()
    {
        TryGetComponent(out agent);
    }

    private void Start()
    {
        // Make agent abale to pass through other agents
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        FindOtherDestination();

        // Debug
        GetComponentInChildren<MeshRenderer>().material.color = isRightSide? Color.green : Color.red;
    }

    private void Update()
    {
        if (window)
            MoveTowardsWindow();

        // Find a new destination when the current one is close enough
        else if (agent.isOnNavMesh && agent.remainingDistance < distanceReachedPoint)
            FindOtherDestination();
    }


    /// <summary>
    /// Initialize the agent
    /// </summary>
    /// <param name="_zone">SpawnZone where the agent will walk</param>
    public void InitializeAgent(TravelerZone _zone, bool _isRightSide)
    {
        reachableBounds = _zone;
        isRightSide = _isRightSide;
        StartMoving();
        FindOtherDestination();
    }

    /// <summary>
    /// Get a new destination for the agent inside its SpawnZone
    /// </summary>
    private void FindOtherDestination()
    {
        Vector3 _targetPosition = agentDestination = reachableBounds.GetRandomPoint();

        NavMeshHit _hit;
        NavMesh.SamplePosition(_targetPosition, out _hit, agent.radius, 1);

        agent.SetDestination(_hit.position);
    }

    /// <summary>
    /// Stop the agent from moving
    /// </summary>
    public void StopMoving()
    {
        agent.enabled = false;
    }

    /// <summary>
    /// Start or resume the agent from moving
    /// </summary>
    public void StartMoving()
    {
        agent.enabled = true;
    }

    public void SetWindow(WindowDepot _window)
    {
        StopMoving();
        window = _window;
    }

    private void MoveTowardsWindow()
    {
        transform.position = Vector3.MoveTowards(transform.position, window.transform.position, Time.deltaTime * 3.5f);
        if (Vector3.Distance(transform.position, window.transform.position) < .5f)
        {
            window = null;
            TravelerManager.instance.SetTravelerToPool(this);
        }
    }
}

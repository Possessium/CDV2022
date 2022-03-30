using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Traveler : MonoBehaviour
{
    private NavMeshAgent agent;
    private TravelerZone reachableBounds;
    [SerializeField, Tooltip("Distance when the agent consider its point reached")] private float distanceReachedPoint = 1;

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
        // Find a new destination when the current one is close enough
        if (agent.remainingDistance < distanceReachedPoint)
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
        agent.isStopped = true;
    }

    /// <summary>
    /// Start or resume the agent from moving
    /// </summary>
    public void StartMoving()
    {
        agent.isStopped = false;
    }
}

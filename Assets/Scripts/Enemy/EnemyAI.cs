using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Shooter reference")]
    public EnemyShooter shooter;

    private NavMeshAgent agent;
    private Transform player;
    private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    private bool inVisionRange = false;
    private bool inShootRange = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (inVisionRange && player != null && player.gameObject.activeInHierarchy)
        {
            if (inShootRange)
            {
                agent.ResetPath();
                shooter.SetTarget(player);
            }
            else
            {
                agent.SetDestination(player.position);
                shooter.ClearTarget();
            }
        }
        else
        {
            shooter.ClearTarget();
            Patrol();
        }
    }

    public void SetPatrolPoints(Transform[] points)
    {
        patrolPoints = points;
        currentPatrolIndex = 0;

        if (patrolPoints != null && patrolPoints.Length > 0)
            GoToNextPatrolPoint();
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            GoToNextPatrolPoint();
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }
    
    public void OnPlayerEnteredVision(Transform playerTransform)
    {
        player = playerTransform;
        inVisionRange = true;
    }

    public void OnPlayerExitedVision()
    {
        inVisionRange = false;
        player = null;
    }

    public void OnPlayerEnteredShootRange()
    {
        inShootRange = true;
    }

    public void OnPlayerExitedShootRange()
    {
        inShootRange = false;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Shooter reference")]
    public EnemyShooter shooter;

    public Animator animator;

    private NavMeshAgent agent;
    private Transform player;
    private Transform[] patrolPoints;
    private int currentPatrolIndex = -1;

    private bool inVisionRange = false;
    private bool inShootRange = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (inVisionRange && player != null && player.gameObject.activeInHierarchy)
        {
            if (inShootRange)
            {
                agent.ResetPath();
                animator.SetBool("isRunning", false);
                shooter.SetTarget(player);
            }
            else
            {
                agent.SetDestination(player.position);
                animator.SetBool("isRunning", true);
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
        if (points == null || points.Length == 0)
        {
            return;
        }

        patrolPoints = points;
        currentPatrolIndex = -1;

        // Запускаем патруль на следующем кадре
        StartCoroutine(DelayedPatrolStart());
    }

    private IEnumerator DelayedPatrolStart()
    {
        yield return null;
        GoToNextPatrolPoint();
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0 || agent == null) return;

        // Если дошёл до текущей точки — идём к следующей
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            animator.SetBool("isRunning", false);
            GoToNextPatrolPoint();
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;

        Transform nextPoint = patrolPoints[currentPatrolIndex];
        if (nextPoint != null)
        {
            agent.SetDestination(nextPoint.position);
            animator.SetBool("isRunning", true);
        }
    }

    // Vision trigger callbacks
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

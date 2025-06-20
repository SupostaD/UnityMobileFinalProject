using UnityEngine;

public class AutoCover : MonoBehaviour
{
    public CircleDrawer overCircle;
    public LayerMask overLayer;
    public float HideDistance = 1.2f;
    public float MoveSpeed = 3f;
    public float JoystickThreshold = 0.05f;
    public PlayerRoll PlayerRoll;
    public ControllSchemePlayerManager ControllScheme;
    public Animator animator;

    private IPlayerInput InputProvider;
    private Transform targetCover;
    private bool isMovingToCover = false;

    void Start()
    {
        InputProvider = ControllScheme.PlayerInput;
    }
    void Update()
    {
        if (PlayerRoll.IsRolling())
        {
            isMovingToCover = false;
            return;
        }

        float radius = overCircle.Radius;

        Vector2 inputDirection = InputProvider.GetMovementInput();
        bool isIdle = inputDirection.magnitude < JoystickThreshold;

        if (isIdle)
        {
            Collider[] covers = Physics.OverlapSphere(transform.position, radius * 2, overLayer);

            if (covers.Length > 0)
            {
                targetCover = FindClosestCover(covers);
                if (targetCover != null)
                {
                    isMovingToCover = true;
                }
            }
        }
        else
        {
            isMovingToCover = false;
            targetCover = null;
        }

        if (isMovingToCover && targetCover != null)
        {
            Vector3 directionAwayFromCover = (transform.position - targetCover.position).normalized;
            Vector3 targetPosition = targetCover.position + directionAwayFromCover * HideDistance;

            animator.SetBool("isRunning", true);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, MoveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMovingToCover = false;
                animator.SetBool("isRunning", false);
            }
        }
    }

    Transform FindClosestCover(Collider[] covers)
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var col in covers)
        {
            float dist = (col.transform.position - transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closest = col.transform;
            }
        }

        return closest;
    }
}

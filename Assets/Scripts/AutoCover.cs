using UnityEngine;

public class AutoCover : MonoBehaviour
{
    public CircleDrawer coverCircle;
    public FloatingJoystick joystick;
    public LayerMask coverLayer;
    public float hideDistance = 1.2f;
    public float moveSpeed = 3f;
    public float joystickThreshold = 0.05f;

    private Transform targetCover;
    private bool isMovingToCover = false;

    void Update()
    {
        float radius = coverCircle.Radius;

        // ���������: ����� ����� ��������
        bool isIdle = joystick.Direction.magnitude < joystickThreshold;

        if (isIdle)
        {
            // ���� �������
            Collider[] covers = Physics.OverlapSphere(transform.position, radius * 2, coverLayer);

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
            // ����� ����� ��������� � �������� �������������� ��������
            isMovingToCover = false;
            targetCover = null;
        }

        // ����������� � �������, ���� ��������
        if (isMovingToCover && targetCover != null)
        {
            Vector3 directionAwayFromCover = (transform.position - targetCover.position).normalized;
            Vector3 targetPosition = targetCover.position + directionAwayFromCover * hideDistance;

            // ��������� ������
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ���� ����� �� ������� � �������������
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMovingToCover = false;
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

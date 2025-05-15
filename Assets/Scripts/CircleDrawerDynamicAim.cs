using UnityEngine;

public class CircleDrawerDynamicAim : MonoBehaviour
{
    public Transform player;
    public float maxRadius = 10f;
    public float minRadius = 5f;
    public float changeSpeed = 2f;
    public int segments = 100;

    private LineRenderer lineRenderer;
    private float currentRadius;
    private Vector3 lastPosition;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = segments;

        currentRadius = maxRadius;
        lastPosition = player.position;

        UpdateCircle(currentRadius);
    }

    void Update()
    {
        bool isMoving = (player.position - lastPosition).sqrMagnitude > 0.001f;

        float targetRadius = isMoving ? minRadius : maxRadius;

        currentRadius = Mathf.Lerp(currentRadius, targetRadius, Time.deltaTime * changeSpeed);

        UpdateCircle(currentRadius);

        lastPosition = player.position;

        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
    }

    void UpdateCircle(float radius)
    {
        for (int i = 0; i < segments; i++)
        {
            float angle = ((float)i / segments) * Mathf.PI * 2f;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, 0f, z));
        }
    }
}

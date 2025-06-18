using UnityEngine;

public class CircleDrawerDynamicAim : MonoBehaviour
{
    public Transform Player;
    public PlayerRoll PlayerRoll;
    public ControllSchemePlayerManager ControllScheme;

    private IPlayerInput InputProvider;

    public float MaxRadius = 10f;
    public float MinRadius = 5f;
    public float ChangeSpeed = 2f;
    public int Segments = 100;
    public float RollChangeSpeed = 10f;

    private LineRenderer lineRenderer;
    public float currentRadius;
    
    void Start()
    {
        InputProvider = ControllScheme.PlayerInput;
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = Segments;

        currentRadius = MaxRadius;
        UpdateCircle(currentRadius);
    }

    void Update()
    {
        bool isRolling = PlayerRoll.IsRolling();
        bool isMoving = InputProvider.GetMovementInput().magnitude > 0.05f;

        float targetRadius;

        if (isRolling || isMoving)
        {
            targetRadius = MinRadius;
        }
        else
        {
            targetRadius = MaxRadius;
        }

        float speed = isRolling ? RollChangeSpeed : ChangeSpeed;
        currentRadius = Mathf.Lerp(currentRadius, targetRadius, Time.deltaTime * speed);

        UpdateCircle(currentRadius);

        transform.position = new Vector3(Player.position.x, transform.position.y, Player.position.z);
    }

    void UpdateCircle(float radius)
    {
        for (int i = 0; i < Segments; i++)
        {
            float angle = ((float)i / Segments) * Mathf.PI * 2f;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, 0f, z));
        }
    }
}

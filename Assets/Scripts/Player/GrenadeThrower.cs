using UnityEngine;
using UnityEngine.EventSystems;

public class GrenadeThrower : MonoBehaviour
{
    [Header("References")]
    public FixedJoystick GrenadeJoystick;
    public Transform Player;
    public GameObject TargetCirclePrefab;
    public Transform GrenadePoint;
    public LineRenderer ArcRenderer;
    public GameObject GrenadePrefab;

    [Header("Settings")]
    public float MinDistance = 1f;
    public float MaxDistance = 6f;
    public float ArcHeight = 2f;
    public int ArcPoints = 30;

    private GameObject currentCircle;
    private GameObject heldGrenade;
    private Rigidbody heldRb;
    private bool isAiming = false;
    private bool isHoldingTouch = false;

    void Update()
    {
        Vector3 input = new Vector3(GrenadeJoystick.Horizontal, 0f, GrenadeJoystick.Vertical);

        if (isHoldingTouch)
        {
            if (!isAiming)
            {
                StartAiming();
            }

            UpdateCirclePosition(input);
            DrawArc(GrenadePoint.position, currentCircle.transform.position);

            Vector3 lookDir = currentCircle.transform.position - Player.position;
            lookDir.y = 0f;
            if (lookDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                Player.rotation = Quaternion.Slerp(Player.rotation, targetRot, 0.2f);
            }
        }
        else
        {
            if (isAiming)
            {
                ThrowGrenade();
            }
        }
    }

    void StartAiming()
    {
        isAiming = true;

        if (currentCircle == null)
            currentCircle = Instantiate(TargetCirclePrefab);

        heldGrenade = Instantiate(GrenadePrefab, GrenadePoint.position, Quaternion.identity);
        heldGrenade.transform.SetParent(GrenadePoint);
        heldGrenade.transform.localPosition = Vector3.zero;

        heldRb = heldGrenade.GetComponent<Rigidbody>();
        heldRb.isKinematic = true;
        heldRb.useGravity = false;

        ArcRenderer.enabled = true;
        currentCircle.SetActive(true);
    }

    void ThrowGrenade()
    {
        isAiming = false;

        ArcRenderer.enabled = false;

        if (heldGrenade != null && currentCircle != null)
        {
            heldGrenade.transform.SetParent(null);
            heldRb.isKinematic = false;
            heldRb.useGravity = true;

            float totalTime;
            Vector3 velocity = CalculateArcVelocity(GrenadePoint.position, currentCircle.transform.position, ArcHeight, out totalTime);

            heldRb.linearVelocity = velocity;

            Grenade grenadeScript = heldGrenade.GetComponent<Grenade>();
            grenadeScript.StartFuse();
        }

        if (currentCircle != null)
        {
            Destroy(currentCircle);
            currentCircle = null;
        }

        heldGrenade = null;
        heldRb = null;
    }

    void UpdateCirclePosition(Vector3 inputDirection)
    {
        if (currentCircle == null) return;

        float clampedMagnitude = Mathf.Clamp(inputDirection.magnitude, 0f, 1f);
        float distance = Mathf.Lerp(MinDistance, MaxDistance, clampedMagnitude);
        Vector3 offset = inputDirection.normalized * distance;

        Vector3 targetPos = Player.position + offset;
        currentCircle.transform.position = new Vector3(targetPos.x, Player.position.y, targetPos.z);
    }

    void DrawArc(Vector3 start, Vector3 end)
    {
        float totalTime;
        Vector3 velocity = CalculateArcVelocity(start, end, ArcHeight, out totalTime);

        ArcRenderer.enabled = true;
        ArcRenderer.positionCount = ArcPoints;

        float timeStep = totalTime / (ArcPoints - 1);

        for (int i = 0; i < ArcPoints; i++)
        {
            float t = i * timeStep;
            Vector3 point = start + velocity * t + 0.5f * Physics.gravity * t * t;
            ArcRenderer.SetPosition(i, point);
        }
    }

    Vector3 CalculateArcVelocity(Vector3 start, Vector3 end, float arcHeight, out float totalTime)
    {
        Vector3 displacement = end - start;
        Vector3 displacementXZ = new Vector3(displacement.x, 0f, displacement.z);

        float yOffset = displacement.y;

        float speedY = Mathf.Sqrt(-2 * Physics.gravity.y * arcHeight);
        float timeUp = speedY / -Physics.gravity.y;
        float timeDown = Mathf.Sqrt(2 * (yOffset + arcHeight) / -Physics.gravity.y);

        totalTime = timeUp + timeDown;

        Vector3 velocityY = Vector3.up * speedY;
        Vector3 velocityXZ = displacementXZ / totalTime;

        return velocityXZ + velocityY;
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        isHoldingTouch = true;
    }

    public void OnPointerUp(BaseEventData eventData)
    {
        if (isAiming)
            ThrowGrenade();

        isHoldingTouch = false;
    }
}

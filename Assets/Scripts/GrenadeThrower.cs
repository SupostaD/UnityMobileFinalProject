using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrenadeThrower : MonoBehaviour
{
    [Header("References")]
    public FixedJoystick grenadeJoystick;
    public Transform player;
    public GameObject targetCirclePrefab;
    public Transform grenadePoint;
    public LineRenderer arcRenderer;
    public GameObject grenadePrefab;

    [Header("Settings")]
    public float minDistance = 1f;
    public float maxDistance = 6f;
    public float arcHeight = 2f;
    public int arcPoints = 30;

    private GameObject currentCircle;
    private GameObject heldGrenade;
    private Rigidbody heldRb;
    private bool isAiming = false;
    private bool isHoldingTouch = false;

    void Update()
    {
        Vector3 input = new Vector3(grenadeJoystick.Horizontal, 0f, grenadeJoystick.Vertical);

        if (isHoldingTouch)
        {
            if (!isAiming)
            {
                StartAiming();
            }

            UpdateCirclePosition(input);
            DrawArc(grenadePoint.position, currentCircle.transform.position);

            Vector3 lookDir = currentCircle.transform.position - player.position;
            lookDir.y = 0f;
            if (lookDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                player.rotation = Quaternion.Slerp(player.rotation, targetRot, 0.2f);
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

        // создать цель
        if (currentCircle == null)
            currentCircle = Instantiate(targetCirclePrefab);

        // создать гранату в руке
        heldGrenade = Instantiate(grenadePrefab, grenadePoint.position, Quaternion.identity);
        heldGrenade.transform.SetParent(grenadePoint);
        heldGrenade.transform.localPosition = Vector3.zero;

        heldRb = heldGrenade.GetComponent<Rigidbody>();
        heldRb.isKinematic = true;
        heldRb.useGravity = false;

        arcRenderer.enabled = true;
        currentCircle.SetActive(true);
    }

    void ThrowGrenade()
    {
        isAiming = false;

        arcRenderer.enabled = false;

        if (heldGrenade != null && currentCircle != null)
        {
            // отсоединяем гранату
            heldGrenade.transform.SetParent(null);
            heldRb.isKinematic = false;
            heldRb.useGravity = true;

            float totalTime;
            Vector3 velocity = CalculateArcVelocity(grenadePoint.position, currentCircle.transform.position, arcHeight, out totalTime);

            heldRb.linearVelocity = velocity;

            Grenade grenadeScript = heldGrenade.GetComponent<Grenade>();
            if (grenadeScript != null)
            {
                grenadeScript.StartFuse();
            }
        }

        // убрать цель
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
        float distance = Mathf.Lerp(minDistance, maxDistance, clampedMagnitude);
        Vector3 offset = inputDirection.normalized * distance;

        Vector3 targetPos = player.position + offset;
        currentCircle.transform.position = new Vector3(targetPos.x, player.position.y, targetPos.z);
    }

    void DrawArc(Vector3 start, Vector3 end)
    {
        float totalTime;
        Vector3 velocity = CalculateArcVelocity(start, end, arcHeight, out totalTime);

        arcRenderer.enabled = true;
        arcRenderer.positionCount = arcPoints;

        float timeStep = totalTime / (arcPoints - 1);

        for (int i = 0; i < arcPoints; i++)
        {
            float t = i * timeStep;
            Vector3 point = start + velocity * t + 0.5f * Physics.gravity * t * t;
            arcRenderer.SetPosition(i, point);
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

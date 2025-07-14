using UnityEngine;
using UnityEngine.UI;

public class PlayerRoll : MonoBehaviour
{
    public float RollForce = 10f;
    public float RollDuration = 0.4f;
    public Rigidbody Rb;
    public ControllSchemePlayerManager ControllScheme;
    
    [Header("Cooldown Settings")]
    public float rollCooldown = 2f;
    private float lastRollTime = -Mathf.Infinity;
    private bool isOnCooldown = false;
    
    [Header("UI")]
    public Image ButtonImage;

    private IPlayerInput InputProvider;
    private bool isRolling = false;
    private Vector3 rollDirection;
    private float rollTimer;

    private void Start()
    {
        InputProvider = ControllScheme.PlayerInput;
    }
    void Update()
    {
        if (isRolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0f)
            {
                isRolling = false;
                Rb.linearVelocity = Vector3.zero;
            }
        }

        if (isOnCooldown)
        {
            float timePassed = Time.time - lastRollTime;
            float ratio = Mathf.Clamp01(timePassed / rollCooldown);
            ButtonImage.fillAmount = 1f - ratio;
            
            if (ratio >= 1f)
            {
                isOnCooldown = false;
                ButtonImage.fillAmount = 1f;
            }
        }
    }

    public void TriggerRoll()
    {
        if (isRolling || isOnCooldown) return;

        Vector2 input2D = InputProvider.GetMovementInput();
        Vector3 input = new Vector3(input2D.x, 0f, input2D.y);

        if (input.sqrMagnitude < 0.1f)
        {
            rollDirection = transform.forward.normalized;
        }
        else
        {
            rollDirection = input.normalized;
        }

        isRolling = true;
        rollTimer = RollDuration;

        Rb.linearVelocity = rollDirection * RollForce;
        
        lastRollTime = Time.time;
        isOnCooldown = true;
    }

    public bool IsRolling()
    {
        return isRolling;
    }
}

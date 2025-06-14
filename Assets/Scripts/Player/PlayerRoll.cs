using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRoll : MonoBehaviour
{
    public float RollForce = 10f;
    public float RollDuration = 0.4f;
    public MonoBehaviour inputProvider;
    public Rigidbody Rb;

    private IPlayerInput InputProvider;
    private bool isRolling = false;
    private Vector3 rollDirection;
    private float rollTimer;

    private void Awake()
    {
        InputProvider = inputProvider as IPlayerInput;
        if (InputProvider == null)
        {
            Debug.LogError("Assigned inputProvider does not implement IPlayerInput!");
        }
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
    }

    public void TriggerRoll()
    {
        if (isRolling) return;

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
    }

    public bool IsRolling()
    {
        return isRolling;
    }
}

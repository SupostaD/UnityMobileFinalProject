using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRoll : MonoBehaviour
{
    public float RollForce = 10f;
    public float RollDuration = 0.4f;
    public FloatingJoystick Joystick;
    public Rigidbody Rb;

    private bool isRolling = false;
    private Vector3 rollDirection;
    private float rollTimer;

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

        Vector3 input = new Vector3(Joystick.Horizontal, 0f, Joystick.Vertical);

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

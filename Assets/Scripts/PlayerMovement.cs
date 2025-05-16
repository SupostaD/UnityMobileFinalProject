using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public FloatingJoystick Joystick;
    public Rigidbody Rb;
    public PlayerRoll PlayerRoll;

    private Vector3 moveDirection;

    void Update()
    {
        if (PlayerRoll.IsRolling())
        {
            moveDirection = Vector3.zero;
            return;
        }

        moveDirection = new Vector3(Joystick.Horizontal, 0, Joystick.Vertical);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 0.2f);
        }
    }

    void FixedUpdate()
    {
        if (PlayerRoll.IsRolling())
            return;

        Vector3 move = moveDirection.normalized * MoveSpeed;
        Rb.linearVelocity = new Vector3(move.x, Rb.linearVelocity.y, move.z);
    }
}

using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public Rigidbody Rb;
    public PlayerRoll PlayerRoll;
    public ControllSchemePlayerManager ControllScheme;
    public Animator animator;
    private IPlayerInput playerInput;
    private Vector3 moveDirection;
    
    private float turnspeed = 720f;

    private void Start()
    {
        playerInput = ControllScheme.PlayerInput;
        if (PlayerRoll == null)
            PlayerRoll = GetComponent<PlayerRoll>();
        if (ControllScheme == null)
            ControllScheme = GetComponent<ControllSchemePlayerManager>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (PlayerRoll.IsRolling())
        {
            moveDirection = Vector3.zero;
            return;
        }

        Vector2 input = playerInput.GetMovementInput();
        moveDirection = new Vector3(input.x, 0, input.y);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            moveDirection.Normalize();
            Quaternion target = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                target,
                turnspeed * Time.deltaTime
                );
            animator.SetBool("isRunning", true);
            animator.SetBool("isShooting", false);
        }
        else animator.SetBool("isRunning", false);
    }

    void FixedUpdate()
    {
        if (PlayerRoll.IsRolling())
            return;

        Vector3 move = moveDirection.normalized * MoveSpeed;
        Rb.linearVelocity = new Vector3(move.x, Rb.linearVelocity.y, move.z);
    }
}

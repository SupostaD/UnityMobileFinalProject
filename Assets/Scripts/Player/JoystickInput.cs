using UnityEngine;

public class JoystickInput : MonoBehaviour, IPlayerInput
{
    public FloatingJoystick joystick;

    public Vector2 GetMovementInput()
    {
        return new Vector2(joystick.Horizontal, joystick.Vertical);
    }
}

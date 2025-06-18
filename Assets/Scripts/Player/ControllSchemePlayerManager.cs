using UnityEngine;

public class ControllSchemePlayerManager : MonoBehaviour
{
    public GameObject FloatingJoystck;
    public GameObject Buttons;
    public IPlayerInput PlayerInput;
    public JoystickInput joystick;
    public ButtonInput buttonInput;
    
    private void Awake()
    {
        var scheme = GameManager.Instance.CurrentControlScheme;

        if (scheme == ControlScheme.Joystick)
        {
            FloatingJoystck.SetActive(true);
            Buttons.SetActive(false);
            PlayerInput = joystick;
        }
        else if (scheme == ControlScheme.Buttons)
        {
            FloatingJoystck.SetActive(false);
            Buttons.SetActive(true);
            PlayerInput = buttonInput;
        }
    }
}

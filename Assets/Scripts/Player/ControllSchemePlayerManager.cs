using UnityEngine;

public class ControllSchemePlayerManager : MonoBehaviour
{
    public GameObject FloatingJoystck;
    public GameObject Buttons;
    public IPlayerInput PlayerInput;
    public JoystickInput joystick;
    public ButtonInput buttonInput;
    
    private void OnEnable()
    {
        GameManager.Instance.OnControlSchemeChanged += ApplyControlScheme;
        ApplyControlScheme(GameManager.Instance.CurrentControlScheme);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnControlSchemeChanged -= ApplyControlScheme;
    }

    private void ApplyControlScheme(ControlScheme scheme)
    {
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

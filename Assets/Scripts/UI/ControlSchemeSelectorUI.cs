using UnityEngine;
using UnityEngine.UI;

public class ControlSchemeSelectorUI : MonoBehaviour
{
    public Button joystickButton;
    public Button buttonsButton;

    public Color selectedColor = Color.gray;
    public Color defaultColor = Color.white;

    private void Start()
    {
        UpdateVisual(GameManager.Instance.CurrentControlScheme);
    }

    public void SelectJoystick()
    {
        GameManager.Instance.SetControlScheme(ControlScheme.Joystick);
        UpdateVisual(ControlScheme.Joystick);
    }

    public void SelectButtons()
    {
        GameManager.Instance.SetControlScheme(ControlScheme.Buttons);
        UpdateVisual(ControlScheme.Buttons);
    }

    private void UpdateVisual(ControlScheme selected)
    {
        SetButtonColor(joystickButton, selected == ControlScheme.Joystick);
        SetButtonColor(buttonsButton, selected == ControlScheme.Buttons);
    }

    private void SetButtonColor(Button button, bool selected)
    {
        var colors = button.colors;
        colors.normalColor = selected ? selectedColor : defaultColor;
        colors.highlightedColor = selected ? selectedColor : defaultColor;
        colors.pressedColor = selected ? selectedColor : defaultColor;
        colors.selectedColor = selected ? selectedColor : defaultColor;
        button.colors = colors;
    }
}

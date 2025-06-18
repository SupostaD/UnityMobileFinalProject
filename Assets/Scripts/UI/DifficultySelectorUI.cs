using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectorUI : MonoBehaviour
{
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    public Color selectedColor = Color.gray;
    public Color defaultColor = Color.white;

    private void Start()
    {
        UpdateButtonVisuals(GameManager.Instance.CurrentDifficulty);
    }

    public void OnEasyPressed()
    {
        GameManager.Instance.SetDifficulty(Difficulty.Easy);
        UpdateButtonVisuals(Difficulty.Easy);
    }

    public void OnMediumPressed()
    {
        GameManager.Instance.SetDifficulty(Difficulty.Medium);
        UpdateButtonVisuals(Difficulty.Medium);
    }

    public void OnHardPressed()
    {
        GameManager.Instance.SetDifficulty(Difficulty.Hard);
        UpdateButtonVisuals(Difficulty.Hard);
    }

    private void UpdateButtonVisuals(Difficulty selected)
    {
        SetButtonColor(easyButton, selected == Difficulty.Easy);
        SetButtonColor(mediumButton, selected == Difficulty.Medium);
        SetButtonColor(hardButton, selected == Difficulty.Hard);
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

using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectorUI : MonoBehaviour
{
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    public Color selectedColor = Color.green;
    public Color defaultColor = Color.white;

    private void Start()
    {
        UpdateVisual(GameManager.Instance.CurrentDifficulty);
    }

    public void SelectEasy()
    {
        GameManager.Instance.SetDifficulty(Difficulty.Easy);
        UpdateVisual(Difficulty.Easy);
    }

    public void SelectMedium()
    {
        GameManager.Instance.SetDifficulty(Difficulty.Medium);
        UpdateVisual(Difficulty.Medium);
    }

    public void SelectHard()
    {
        GameManager.Instance.SetDifficulty(Difficulty.Hard);
        UpdateVisual(Difficulty.Hard);
    }

    private void UpdateVisual(Difficulty selected)
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

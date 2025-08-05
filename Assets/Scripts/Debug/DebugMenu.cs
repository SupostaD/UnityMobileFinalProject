using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    public static bool isGodMode;
    
    public TMP_InputField nameInputField;
    public TMP_Text warningText;
    
    private const int MaxScoreLength = 5;
    private static readonly Regex validScoreRegex = new Regex(@"^\d+$");

    public TextMeshProUGUI godmodeButtonText;

    public void Awake()
    {
        godmodeButtonText.text = "Off";
        isGodMode = false;
    }

    public void SetEasyDifficulty()
    {
        GameManager.Instance.SetDifficulty(Difficulty.Easy);
    }

    public void SetMediumDifficulty()
    {
        GameManager.Instance.SetDifficulty(Difficulty.Medium);
    }

    public void SetHardDifficulty()
    {
        GameManager.Instance.SetDifficulty(Difficulty.Hard);
    }

    public void ToggleGodMode()
    {
        isGodMode = !isGodMode;
        if (isGodMode)
        {
            godmodeButtonText.text = "On";
        }
        else godmodeButtonText.text = "Off";
    }

    public void DebugMenuSetScore()
    {
        string enteredScore = nameInputField.text.Trim();

        if (enteredScore.Length > MaxScoreLength || !validScoreRegex.IsMatch(enteredScore))
        {
            warningText.text = "Score must be a positive number (max 5 digits)";
        }
        else
        {
            warningText.text = ""; 
            int EnteredScore = int.Parse(enteredScore);
            GameManager.Instance.SetScore(EnteredScore);
            UIManager.Instance.UpdateScore(EnteredScore);
        }
    }
}

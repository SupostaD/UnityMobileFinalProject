using TMPro;
using UnityEngine;

public class VibrationToggleButton : MonoBehaviour
{
    public TMP_Text buttonText; // Назначь в инспекторе

    private void Start()
    {
        UpdateText();
    }

    public void ToggleVibration()
    {
        VibrationSettings.IsVibrationEnabled = !VibrationSettings.IsVibrationEnabled;
        UpdateText();
    }

    private void UpdateText()
    {
        buttonText.text = VibrationSettings.IsVibrationEnabled ? "Vibration: ON" : "Vibration: OFF";
    }
}

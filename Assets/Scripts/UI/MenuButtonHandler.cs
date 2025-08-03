using UnityEngine;

public class MenuButtonHandler : MonoBehaviour
{
    public void OnConfirmButtonPressed()
    {
        VibrationManager.VibrateConfirm();
    }
}

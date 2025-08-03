using Effects;
using UnityEngine;

public static class VibrationManager
{
    public static void VibrateConfirm()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!VibrationSettings.IsVibrationEnabled) return;
        Vibration.Vibrate(30, 100);
#endif
    }

    public static void VibrateSuccess()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!VibrationSettings.IsVibrationEnabled) return;
        Vibration.Vibrate(60, 180);
#endif
    }

    public static void VibrateFailure()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!VibrationSettings.IsVibrationEnabled) return;
        Vibration.Vibrate(100, 255);
#endif
    }
}

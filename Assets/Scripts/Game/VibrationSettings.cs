using UnityEngine;

public static class VibrationSettings
{
    private const string VibrationKey = "VibrationEnabled";

    public static bool IsVibrationEnabled
    {
        get => PlayerPrefs.GetInt(VibrationKey, 1) == 1;
        set => PlayerPrefs.SetInt(VibrationKey, value ? 1 : 0);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DebugGestureDetector : MonoBehaviour
{
    public GameObject debugPanel;
    private List<float> tapTimes = new();
    public float gestureTimeLimit = 3f;
    private int requiredTaps = 5;

    private void Awake()
    {
        debugPanel.SetActive(false);
    }

    private void Update()
    {
        if (!IsInGameplay()) return;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 tapPos = Touchscreen.current.primaryTouch.position.ReadValue();

            if (tapPos.x < Screen.width * 0.2f && tapPos.y > Screen.height * 0.8f)
            {
                RegisterTap();
            }
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            if (mousePos.x < Screen.width * 0.2f && mousePos.y > Screen.height * 0.8f)
            {
                RegisterTap();
            }
        }
    }

    private void RegisterTap()
    {
        float now = Time.time;
        tapTimes.Add(now);
        tapTimes.RemoveAll(t => now - t > gestureTimeLimit);

        if (tapTimes.Count >= requiredTaps)
        {
            ShowDebugPanel();
            tapTimes.Clear();
        }
    }

    private void ShowDebugPanel()
    {
        Time.timeScale = 0f;
        debugPanel.SetActive(true);
        AudioManager.Instance.SetBGMVolume(0.5f);
    }

    public void HideDebugPanel()
    {
        Time.timeScale = 1f;
        debugPanel.SetActive(false);
        AudioManager.Instance.SetBGMVolume(1f);
    }

    private bool IsInGameplay()
    {
        return SceneManager.GetActiveScene().name != "MainMenu";
    }
}

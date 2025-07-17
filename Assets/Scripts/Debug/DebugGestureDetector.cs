using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 tapPos = Input.mousePosition;
            if (tapPos.x < Screen.width * 0.2f && tapPos.y > Screen.height * 0.8f)
                RegisterTap();
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 tapPos = Input.GetTouch(0).position;
            if (tapPos.x < Screen.width * 0.2f && tapPos.y > Screen.height * 0.8f)
                RegisterTap();
        }
#endif
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
    }

    public void HideDebugPanel()
    {
        Time.timeScale = 1f;
        debugPanel.SetActive(false);
    }

    private bool IsInGameplay()
    {
        return SceneManager.GetActiveScene().name != "MainMenu";
    }
}

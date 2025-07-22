using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine;
using System.Globalization;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AnalyticsService.Instance.StartDataCollection();
    }

    public void StartSession()
    {
        var evt = new CustomEvent("session_start");
        evt.Add("time", System.DateTime.UtcNow.ToString("o"));
        AddInfo(evt);
        AnalyticsService.Instance.RecordEvent(evt);
    }

    private void OnApplicationQuit()
    {
        var evt = new CustomEvent("user_quit_game");
        evt.Add("time", System.DateTime.UtcNow.ToString("o"));
        AddInfo(evt);
        AnalyticsService.Instance.RecordEvent(evt);
    }

    public void LogAchievement(string id, int streak)
    {
        var evt = new CustomEvent("achievement_unlocked");
        evt.Add("achievement", id);
        evt.Add("streak_day", streak);
        AddInfo(evt);
        AnalyticsService.Instance.RecordEvent(evt);
    }
    
    string GetCountryCode()
    {
        return RegionInfo.CurrentRegion.TwoLetterISORegionName;
    }

    void AddInfo(CustomEvent custEvent)
    {
        custEvent.Add("clientVersion", Application.version);
        custEvent.Add("platform", Application.platform.ToString());
        custEvent.Add("sdkMethod", "AnalyticsService.RecordEvent");
        custEvent.Add("userCountry", GetCountryCode());
    }
}

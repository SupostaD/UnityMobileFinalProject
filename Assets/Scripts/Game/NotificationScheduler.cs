using UnityEngine;
using Unity.Notifications.Android;
using System;
using System.Collections;

public class NotificationScheduler  : MonoBehaviour
{
    private const string ChannelId = "reward_channel";

    IEnumerator Start()
    {
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
            yield return null;

        Debug.Log($"Permission request status: {request.Status}");
        if (request.Status != PermissionStatus.Allowed)
        {
            Debug.LogWarning("Notification permission NOT granted — уведомления не будут показаны.");
            yield break;
        }

        RegisterChannel();

        if (ShouldScheduleNotification())
            ScheduleNotification();
    }

    void RegisterChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = ChannelId,
            Name = "Reward Notifications",
            Importance = Importance.Default,
            Description = "Daily reward reminders",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        Debug.Log("Channel registered");
    }

    bool ShouldScheduleNotification()
    {
        if (!PlayerPrefs.HasKey("LastLoginTime")) return true;

        DateTime last = DateTime.Parse(PlayerPrefs.GetString("LastLoginTime"));
        return (DateTime.Now - last).TotalHours >= 1;
    }

    void ScheduleNotification()
    {
        var notification = new AndroidNotification
        {
            Title = "Come back!",
            Text = "Your damage boost awaits you!",
            FireTime = DateTime.Now.AddHours(24),
        };

        AndroidNotificationCenter.SendNotification(notification, ChannelId);
        PlayerPrefs.SetString("LastLoginTime", DateTime.Now.ToString());
        Debug.Log("Scheduled for " + notification.FireTime);
    }
}

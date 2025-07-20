using System.IO;
using UnityEngine;

public static  class DailyRewardStorage 
{
    private static string GetFolder()
    {
        string path = Path.Combine(Application.persistentDataPath, "daily_rewards");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        return path;
    }
    private static string GetPath(string playerName)
    {
        return Path.Combine(GetFolder(), $"daily_reward_{playerName}.json");
    }

    public static void Save(DailyRewardData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(data.playerName), json);
    }

    public static DailyRewardData Load(string playerName)
    {
        string path = GetPath(playerName);
        if (!File.Exists(path))
            return new DailyRewardData { playerName = playerName, streakDay = 1, lastClaimDate = "" };

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<DailyRewardData>(json);
    }
}

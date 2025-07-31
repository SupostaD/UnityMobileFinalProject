using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadController : MonoBehaviour
{
    public static SaveLoadController Instance { get; private set; }
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

    public void SaveGame(string saveFileName)
    {
        SaveData data = new SaveData();
        data.playerName = GameManager.Instance.PlayerName;
        data.difficulty = GameManager.Instance.CurrentDifficulty.ToString();
        data.controlScheme = GameManager.Instance.CurrentControlScheme.ToString();
        data.score = GameManager.Instance.Score;

        var player = GameObject.FindGameObjectWithTag("Player");
        data.playerPosition = player.transform.position;
        data.playerRotation = player.transform.rotation;
        data.playerHP = player.GetComponent<Health>().currentHealth;

        data.dailyRewardStreak = GameManager.Instance.DailyRewardStreak;
        data.lastRewardClaimDate = GameManager.Instance.LastRewardClaimDate;

        var roll = player.GetComponent<PlayerRoll>();
        if (roll != null)
            data.rollCooldown = roll.GetCooldownRoll();

        var grenadeThrower = player.GetComponent<GrenadeThrower>();
        if (grenadeThrower != null)
            data.grenadeCooldown = grenadeThrower.GetCooldownGrenade();

        var bulletsInMagazine = player.GetComponent<AutoShooter>();
        if (bulletsInMagazine != null)
        {
            data.bulletsInMagazine = bulletsInMagazine.GetBulletCount();
            data.weaponCooldown = bulletsInMagazine.GetCooldown();
        }

        data.elapsedTime = GameManager.Instance.ElapsedTime;
        data.nameScene = SceneManager.GetActiveScene().name;

        data.enemies = new List<EnemyData>();
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            EnemyData e = new EnemyData
            {
                Position = enemy.transform.position,
                Rotation = enemy.transform.rotation,
                Hp = enemy.CurrentHP
            };
            data.enemies.Add(e);
        }

        data.bulletsFromPlayer = new List<BulletSaveData>();
        foreach (var bulletFromPlayer in FindObjectsOfType<BulletMovement>())
            data.bulletsFromPlayer.Add(bulletFromPlayer.GetSaveData());

        data.bulletsFromEnemy = new List<BulletSaveData>();
        foreach (var bulletFromEnemy in FindObjectsOfType<EnemyBullet>())
            data.bulletsFromEnemy.Add(bulletFromEnemy.GetSaveData());

        var grenade = FindFirstObjectByType<Grenade>();
        if (grenade != null)
        {
            data.hasActiveGrenade = true;
            data.activeGrenade = grenade.GetSaveData();
        }
        else
        {
            data.hasActiveGrenade = false;
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, saveFileName + ".json"), json);
    }

    public void LoadGame(string saveFileName)
    {
        string path = Path.Combine(Application.persistentDataPath, saveFileName + ".json");
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        GameManager.Instance.SetPendingLoadData(data);
        GameManager.Instance.DailyRewardStreak = data.dailyRewardStreak;
        GameManager.Instance.LastRewardClaimDate = data.lastRewardClaimDate;

        SceneTransitionManager.Instance.TransitionToScene(data.nameScene);
    }
}

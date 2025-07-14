using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadController : MonoBehaviour
{
    public void SaveGame(string saveFileName)
    {
        SaveData data = new SaveData();
        data.playerName = GameManager.Instance.PlayerName;
        data.difficulty = GameManager.Instance.CurrentDifficulty.ToString();
        data.controlScheme = GameManager.Instance.CurrentControlScheme.ToString();
        data.score = GameManager.Instance.Score;
        var player = GameObject.FindGameObjectWithTag("Player");
        data.playerPosition = player.transform.position;
        data.playerHP = player.GetComponent<Health>().currentHealth;
        data.elapsedTime = GameManager.Instance.ElapsedTime;
        data.nameScene = SceneManager.GetActiveScene().name;

        data.enemies = new List<EnemyData>();
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            EnemyData e = new EnemyData
            {
                EnemyId = enemy.ID,
                Position = enemy.transform.position,
                Hp = enemy.CurrentHP
            };
            data.enemies.Add(e);
        }
        
        data.bullets = new List<BulletSaveData>();
        foreach (var bullet in FindObjectsOfType<BulletMovement>())
        {
            data.bullets.Add(bullet.GetSaveData());
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

        SceneManager.LoadScene(data.nameScene);
    }
}

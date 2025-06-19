using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

        data.enemies = new List<EnemyData>();
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            EnemyData e = new EnemyData
            {
                enemyId = enemy.ID,
                position = enemy.transform.position,
                hp = enemy.CurrentHP
            };
            data.enemies.Add(e);
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

        GameManager.Instance.SetPlayerName(data.playerName);
        GameManager.Instance.SetDifficulty((Difficulty)Enum.Parse(typeof(Difficulty), data.difficulty));
        GameManager.Instance.SetControlScheme((ControlScheme)Enum.Parse(typeof(ControlScheme), data.controlScheme));
        GameManager.Instance.SetScore(data.score);
        
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = data.playerPosition;
        player.GetComponent<Health>().currentHealth = data.playerHP;

        foreach (var enemy in data.enemies)
        {
            Enemy found = Enemy.FindById(enemy.enemyId);
            if (found != null)
            {
                found.transform.position = enemy.position;
                found.GetComponent<Health>().SetHealth(enemy.hp);;
            }
        }
    }
}

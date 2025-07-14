using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string playerName;
    public string difficulty;
    public string controlScheme;
    public int score;
    public Vector3 playerPosition;
    public int playerHP;
    public List<EnemyData> enemies;
    public float elapsedTime;
    public string nameScene;
    public List<BulletSaveData> bullets;
}

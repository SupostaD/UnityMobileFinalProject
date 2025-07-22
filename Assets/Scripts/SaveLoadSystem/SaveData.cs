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
    public Quaternion playerRotation;
    public int playerHP;
    public List<EnemyData> enemies;
    public float elapsedTime;
    public string nameScene;
    public List<BulletSaveData> bulletsFromPlayer;
    public List<BulletSaveData> bulletsFromEnemy;
    public float weaponCooldown;     
    public int bulletsInMagazine;    
    public float rollCooldown;       
    public float grenadeCooldown;
    public bool hasActiveGrenade;
    public GrenadeSaveData activeGrenade;
    public int dailyRewardStreak = 1;
    public string lastRewardClaimDate = "";
}

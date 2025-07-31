using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Health health;

    public int CurrentHP => health != null ? health.currentHealth : 0;

    private void Awake()
    {
        health = GetComponent<Health>();
    }
    
    /*private void Start()
    {
        health = GetComponent<Health>();
    }*/
    
    public void ApplySaveData(EnemyData data)
    {
        transform.position = data.Position;
        health.SetHealth(data.Hp);
    }
}

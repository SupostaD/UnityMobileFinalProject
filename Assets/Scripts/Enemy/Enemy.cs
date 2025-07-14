using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string uniqueId;
    private Health health;

    public string ID => uniqueId;
    public int CurrentHP => health != null ? health.currentHealth : 0;

    private void Awake()
    {
        health = GetComponent<Health>();
    }
    
    private void Start()
    {
        if (string.IsNullOrEmpty(uniqueId))
            uniqueId = Guid.NewGuid().ToString();
        
        health = GetComponent<Health>();
   }
    
    public void SetId(string id)
    {
        uniqueId = id;
    }
    
    public static Enemy FindById(string id)
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            if (enemy.ID == id)
                return enemy;
        }
        return null;
    }
    
    public void ApplySaveData(EnemyData data)
    {
        transform.position = data.Position;
        health.currentHealth = data.Hp;
    }
}

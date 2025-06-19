using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string uniqueId;
    private Health health;

    public string ID => uniqueId;
    public int CurrentHP => health != null ? health.currentHealth : 0;

    private void Start()
    {
        if (string.IsNullOrEmpty(uniqueId))
            uniqueId = Guid.NewGuid().ToString();
        
        health = GetComponent<Health>();
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
}

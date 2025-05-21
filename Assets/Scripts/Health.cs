using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public EnemyStats stats;
    public Difficulty difficulty = Difficulty.Easy;
    public int manualMaxHealth = 10;
    
    private int maxHealth;
    private int currentHealth;

    public UnityEvent onDeath;
    public UnityEvent<float> onHealthChanged;

    void Start()
    {
        if (stats != null)
            maxHealth = stats.GetHealthByDifficulty(difficulty);
        else
            maxHealth = manualMaxHealth;
        
        currentHealth = maxHealth;
        onHealthChanged.Invoke(1f);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged.Invoke((float)currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            onDeath.Invoke();
        }
    }
}

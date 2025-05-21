using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    public UnityEvent onDeath;
    public UnityEvent<float> onHealthChanged; // % здоровья от 0 до 1

    void Start()
    {
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

using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public EnemyStats stats;
    private int maxHealth;
    public int currentHealth;
    private int score;

    public UnityEvent OnDeath;
    public UnityEvent<float> OnHealthChanged;

    private IHealthUIUpdater[] uiElements;

    void Start()
    {
        var difficulty = GameManager.Instance.CurrentDifficulty;

        if (gameObject.CompareTag("Player"))
            maxHealth = 100;
        else if (gameObject.CompareTag("Enemy"))
            maxHealth = stats.GetHealthByDifficulty(difficulty);

        score = stats.GetScoreByDifficulty(difficulty);

        currentHealth = maxHealth;

        uiElements = GetComponentsInChildren<IHealthUIUpdater>();

        UpdateAllUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateAllUI();

        if (currentHealth <= 0)
        {
            GameManager.Instance.AddScore(score);
            OnDeath?.Invoke();
        }
    }

    private void UpdateAllUI()
    {
        float normalized = (float)currentHealth / maxHealth;
        OnHealthChanged?.Invoke(normalized);

        foreach (var ui in uiElements)
        {
            ui.UpdateUI(normalized);
        }
    }
}

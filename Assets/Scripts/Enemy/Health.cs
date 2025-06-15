using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public EnemyStats stats;
    public Difficulty difficulty = Difficulty.Easy;
    public int manualMaxHealth = 10;
    public int manualScore = 10;

    private int maxHealth;
    private int currentHealth;
    private int score;

    public UnityEvent OnDeath;
    public UnityEvent<float> OnHealthChanged;

    private IHealthUIUpdater[] uiElements;

    void Start()
    {
        if (stats != null)
        {
            maxHealth = stats.GetHealthByDifficulty(difficulty);
            score = stats.GetScoreByDifficulty(difficulty);
        }
        else
        {
            maxHealth = manualMaxHealth;
            score = manualScore;
        }

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

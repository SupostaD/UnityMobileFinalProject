using System;
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
    public bool OverrideHealthFromSave = false;

    private IHealthUIUpdater[] uiElements;

    void Start()
    {
        var difficulty = GameManager.Instance.CurrentDifficulty;

        if (gameObject.CompareTag("Player"))
            maxHealth = 100;
        else if (gameObject.CompareTag("Enemy"))
            maxHealth = stats.GetHealthByDifficulty(difficulty);

        score = stats.GetScoreByDifficulty(difficulty);

        if (!OverrideHealthFromSave)
            currentHealth = maxHealth;

        uiElements = GetComponentsInChildren<IHealthUIUpdater>();

        UpdateAllUI();
    }

    public void TakeDamage(int amount)
    {
        if (DebugMenu.isGodMode && gameObject.CompareTag("Player"))
            return;
        
        if (!DebugMenu.isGodMode || (DebugMenu.isGodMode && gameObject.CompareTag("Enemy")))
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            UpdateAllUI();
        }

        if (currentHealth <= 0 && gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.AddScore(score);
            AudioManager.Instance.PlaySFX("Death");
            OnDeath?.Invoke();
        }
        else if (currentHealth <= 0 && gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX("Death");
            OnDeath?.Invoke();
        }
    }

    private void UpdateAllUI()
    {
        float normalized = (float)currentHealth / maxHealth;
        OnHealthChanged?.Invoke(normalized);

        if (uiElements != null)
        {
            foreach (var ui in uiElements)
            {
                ui.UpdateUI(normalized);
            }   
        }
    }
    
    public void SetHealth(int value)
    {
        currentHealth = value;
        OverrideHealthFromSave = true;
        UpdateAllUI();

        if (currentHealth <= 0)
            OnDeath?.Invoke();
    }
}

using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public System.Action<Enemy> OnDeath;
    
    private int currentHealth;
    private bool isVisible;

    private void OnEnable()
    {
        currentHealth = enemyType.health;
        isVisible = false;
        GetComponent<Renderer>().enabled = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        OnDeath?.Invoke(this);
        gameObject.SetActive(false);
    }

    private void OnBecameVisible()
    {
        isVisible = true;
        GetComponent<Renderer>().enabled = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
}

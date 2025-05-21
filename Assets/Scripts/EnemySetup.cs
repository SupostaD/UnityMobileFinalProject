using UnityEngine;

public class EnemySetup : MonoBehaviour
{
    public GameObject healthBarPrefab;

    void Start()
    {
        GameObject hb = Instantiate(healthBarPrefab);
        EnemyHealthBar bar = hb.GetComponent<EnemyHealthBar>();
        bar.target = transform;

        Health health = GetComponent<Health>();
        health.onHealthChanged.AddListener(bar.SetHealth);
        health.onDeath.AddListener(() =>
        {
            Destroy(hb);
            Destroy(gameObject);
        });
    }
}

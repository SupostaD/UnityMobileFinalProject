using UnityEngine;

public class BulletMovement : BaseBullet
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health health = other.GetComponent<Health>();
            int finalDamage = DamageMultiplierManager.Apply(Damage);
            health.TakeDamage(finalDamage);
            Destroy(gameObject);
        }
    }
}

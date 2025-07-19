using UnityEngine;

public class BulletMovement : BaseBullet
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(Damage);

            Destroy(gameObject);
        }
    }
}

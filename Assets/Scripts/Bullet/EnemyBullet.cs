using UnityEngine;

public class EnemyBullet : BaseBullet
{
    void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(Deactivate), LifeTime);
    }

    void Deactivate()
    {
        EnemyBulletPool.Instance.ReturnBullet(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.gameObject;

        if (hitObject.CompareTag("Player"))
        {
            Health hp = collision.collider.GetComponent<Health>();
            if (hp != null)
                hp.TakeDamage(Damage);
        }

        EnemyBulletPool.Instance.ReturnBullet(gameObject);
    }
}

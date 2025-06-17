using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float LifeTime = 5f;
    public int Damage = 1;

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

            EnemyBulletPool.Instance.ReturnBullet(gameObject);
        }
        else EnemyBulletPool.Instance.ReturnBullet(gameObject);
    }
}

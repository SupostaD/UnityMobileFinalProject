using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float LifeTime = 5f;
    public int Damage = 5;

    void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(Deactivate), LifeTime);
    }

    void Deactivate()
    {
        PlayerBulletPool.Instance.ReturnBullet(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.gameObject;
        
        if (hitObject.CompareTag("Enemy"))
        {
            Health hp = hitObject.GetComponent<Health>();
            if (hp != null)
                hp.TakeDamage(Damage);
            
            PlayerBulletPool.Instance.ReturnBullet(gameObject);

        } 
        else PlayerBulletPool.Instance.ReturnBullet(gameObject);

    }
}
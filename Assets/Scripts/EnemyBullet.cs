using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float LifeTime = 5f;

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
            Debug.Log("Bullet hit a player");
            EnemyBulletPool.Instance.ReturnBullet(gameObject);
        }
        else if (hitObject.CompareTag("Bullet"))
        {
            
        }
        else EnemyBulletPool.Instance.ReturnBullet(gameObject);
    }
}
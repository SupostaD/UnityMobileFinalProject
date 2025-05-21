using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float LifeTime = 5f;
    public int Damage = 1;

    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player received damage!");

            Health hp = collision.collider.GetComponent<Health>();
            if (hp != null)
                hp.TakeDamage(Damage);

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

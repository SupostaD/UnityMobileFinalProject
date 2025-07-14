using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float LifeTime = 3f;
    public int Damage = 1;
    public 

    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(Damage);
            }

            Destroy(gameObject);
        }
    }
    
    public BulletSaveData GetSaveData()
    {
        return new BulletSaveData
        {
            position = transform.position,
            rotation = transform.rotation
        };
    }
}

[System.Serializable]
public struct BulletSaveData
{
    public Vector3 position;
    public Quaternion rotation;
}

using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float LifeTime = 3f;
    public int Damage = 1;

    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}

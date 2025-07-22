using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public float LifeTime = 3f;
    public int Damage = 1;

    protected virtual void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    public BulletSaveData GetSaveData()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        return new BulletSaveData
        {
            position = transform.position,
            rotation = transform.rotation,
            velocity = rb != null ? rb.linearVelocity : Vector3.zero
        };
    }

    public void ApplySaveData(BulletSaveData data)
    {
        transform.position = data.position;
        transform.rotation = data.rotation;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = data.velocity;
    }
}

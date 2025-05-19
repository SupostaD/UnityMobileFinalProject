using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float LifeTime = 5f;

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
            hitObject.SetActive(false);
            PlayerBulletPool.Instance.ReturnBullet(gameObject);

        } 
        else if (hitObject.CompareTag("Bullet"))
        {
            Debug.Log("Bullet collided with a bullet");
        }
        else PlayerBulletPool.Instance.ReturnBullet(gameObject);

    }
}
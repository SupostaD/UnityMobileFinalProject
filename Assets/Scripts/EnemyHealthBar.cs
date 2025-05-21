using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Transform target;
    public Image fillImage;
    public Vector3 offset = new Vector3(0, 2f, 0);

    void Update()
    {
        transform.position = target.position + offset;
        transform.forward = Camera.main.transform.forward;
    }

    public void SetHealth(float value)
    {
        fillImage.fillAmount = value;
    }
}

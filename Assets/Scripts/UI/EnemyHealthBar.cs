using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour, IHealthUIUpdater
{
    public Transform target;
    public Image fillImage;
    public Vector3 offset = new Vector3(0, 2f, 0);

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.forward = Camera.main.transform.forward;
        }
    }

    public void UpdateUI(float value)
    {
        fillImage.fillAmount = value;
    }
}

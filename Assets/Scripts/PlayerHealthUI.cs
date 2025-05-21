using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Health playerHealth;
    public Image fillImage;

    void Start()
    {
        playerHealth.onHealthChanged.AddListener(UpdateUI);
    }

    void UpdateUI(float value)
    {
        fillImage.fillAmount = value;
    }
}

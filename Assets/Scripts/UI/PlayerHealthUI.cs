using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour, IHealthUIUpdater
{
    public Image fillImage;

    public void UpdateUI(float value)
    {
        fillImage.fillAmount = value;
    }
}

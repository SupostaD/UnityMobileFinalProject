using UnityEngine;
using UnityEngine.UI;

public class RewardPopup : MonoBehaviour
{
    public Image[] giftImages;
    public Sprite lockedGiftSprite;
    public Sprite normalGiftSprite;
    public Sprite openedGiftSprite;

    /// <summary>
    /// Обновляет отображение стрийка в попапе
    /// </summary>
    /// <param name="streakDay">От 1 до 7</param>
    public void SetupVisuals(int streakDay)
    {
        for (int i = 0; i < giftImages.Length; i++)
        {
            if (i < streakDay - 1)
            {
                giftImages[i].sprite = openedGiftSprite;
            }
            else if (i == streakDay - 1)
            {
                giftImages[i].sprite = normalGiftSprite;
            }
            else
            {
                giftImages[i].sprite = lockedGiftSprite;
            }
        }
    }
}

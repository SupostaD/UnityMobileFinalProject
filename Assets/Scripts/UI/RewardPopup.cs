using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopup : MonoBehaviour
{
    public Image[] giftImages;
    public Sprite lockedGiftSprite;
    public Sprite normalGiftSprite;
    public Sprite openedGiftSprite;
    
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
    
    public IEnumerator AnimateButton(Button btn)
    {
        Vector3 original = btn.transform.localScale;
        Vector3 target = original * 1.1f;

        while (true)
        {
            btn.transform.localScale = Vector3.Lerp(original, target, Mathf.PingPong(Time.time * 2, 1));
            yield return null;
        }
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardManager : MonoBehaviour
{
    public static DailyRewardManager Instance { get; private set; }

    [Header("Popup Prefab")]
    public GameObject rewardPopupPrefab;

    private DailyRewardData rewardData;
    private Action onRewardClaimed;

    private GameObject activePopup;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CheckAndShowReward(string playerName, Action onClaimed)
    {
        rewardData = DailyRewardStorage.Load(playerName);
        onRewardClaimed = onClaimed;

        if (AlreadyClaimedToday())
        {
            Debug.Log("The award has already been received today.");
            onRewardClaimed?.Invoke();
            return;
        }

        if (Has24HoursPassed())
        {
            ShowRewardPopup();
        }
        else
        {
            onRewardClaimed?.Invoke();
        }
    }

    private bool AlreadyClaimedToday()
    {
        if (string.IsNullOrEmpty(rewardData.lastClaimDate)) return false;

        DateTime lastClaim = DateTime.Parse(rewardData.lastClaimDate);
        return lastClaim.Date == DateTime.Now.Date;
    }

    private bool Has24HoursPassed()
    {
        if (string.IsNullOrEmpty(rewardData.lastClaimDate)) return true;

        DateTime lastClaim = DateTime.Parse(rewardData.lastClaimDate);
        return (DateTime.Now - lastClaim).TotalHours >= 24; //.TotalMinutes >= 5 - для теста TotalHours >= 24 - для билда
    }

    private void ShowRewardPopup()
    {
        activePopup = Instantiate(rewardPopupPrefab);
        var ui = activePopup.GetComponent<RewardPopup>();
        ui.SetupVisuals(rewardData.streakDay);

        Button claimButton = activePopup.GetComponentInChildren<Button>();

        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(ClaimReward);
    }

    private void ClaimReward()
    {
        rewardData.lastClaimDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        rewardData.streakDay = Mathf.Min(rewardData.streakDay + 1, 7);

        DailyRewardStorage.Save(rewardData);
        
        DamageMultiplierManager.IsDoubleDamageActive = true;
        StartCoroutine(DisableDoubleDamageAfterSeconds(60));

        Debug.Log($"Awarded. Streak:{rewardData.streakDay}");

        if (activePopup != null)
            Destroy(activePopup);

        onRewardClaimed?.Invoke();
    }
    
    private IEnumerator DisableDoubleDamageAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DamageMultiplierManager.IsDoubleDamageActive = false;
        Debug.Log("Double damage is over");
    }
}

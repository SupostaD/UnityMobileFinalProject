using System;
using System.Collections;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class SaveSlotButton : MonoBehaviour
{
    public TMP_Text slotLabel;
    private string saveFileName;
    public RawImage previewImage;

    private string playerName;

    public void Setup(SaveData data, string fileName)
    {
        saveFileName = fileName;
        playerName = data.playerName;

        slotLabel.text =
            $"{data.playerName} " +
            $"{data.difficulty} " +
            $"{data.controlScheme} " +
            $"{FormatTime(data.elapsedTime)}\n" +
            $"Score: {data.score}";

        string previewPath = Path.Combine(Application.persistentDataPath, fileName + "_preview.png");
        if (File.Exists(previewPath))
        {
            byte[] bytes = File.ReadAllBytes(previewPath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            previewImage.texture = tex;
        }
        else
        {
            previewImage.texture = null;
        }
    }

    public void OnClick()
    {
        DailyRewardManager rewardManager = FindAnyObjectByType<DailyRewardManager>();
        rewardManager.CheckAndShowReward(playerName, () =>
        {
            //SaveLoadController.Instance.LoadGame(saveFileName);
            StartCoroutine(DelayedLoad());        
        });
    }
    
    private IEnumerator DelayedLoad()
    {
        yield return null; // дождись конца текущего кадра
        SaveLoadController.Instance.LoadGame(saveFileName);
    }

    private string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60);
        int secs = Mathf.FloorToInt(seconds % 60);
        return $"{mins:D2}:{secs:D2}";
    }
}

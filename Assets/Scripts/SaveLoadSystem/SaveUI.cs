using System;
using UnityEngine;

public class SaveUI : MonoBehaviour
{
    public SaveSlotListUI slotListUI;

    public void SaveNewGame()
    {
        string slotName = "Save_" + DateTime.Now.Ticks;

        Texture2D tex = ScreenshotUtility.Instance.Capture();
        ScreenshotUtility.Instance.SaveTextureToPNG(tex, slotName + "_preview.png");

        SaveLoadController.Instance.SaveGame(slotName);
        slotListUI.LoadAllSlots();
    }
}

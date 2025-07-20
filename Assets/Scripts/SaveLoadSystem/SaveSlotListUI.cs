using System.Collections;
using UnityEngine;
using System.IO;

public class SaveSlotListUI : MonoBehaviour
{
    public GameObject slotButtonPrefab;
    public Transform slotParent;
    public SaveLoadController saveLoader;

    void OnEnable()
    {
        StartCoroutine(LoadSlotsDelayed());
    }

    IEnumerator LoadSlotsDelayed()
    {
        yield return null;
        yield return null;
        LoadAllSlots();
    }

    public void LoadAllSlots()
    {
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        string[] saveFiles = Directory.GetFiles(Application.persistentDataPath, "*.json");
        foreach (string path in saveFiles)
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            string fileName = Path.GetFileNameWithoutExtension(path);

            GameObject slotGO = Instantiate(slotButtonPrefab, slotParent);
            var slot = slotGO.GetComponent<SaveSlotButton>();
            slot.Setup(data, fileName, saveLoader);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GlobalButtonSFX : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        HookAllButtons();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HookAllButtons();
    }

    private void HookAllButtons()
    {
        foreach (var button in FindObjectsOfType<Button>(includeInactive: true))
        {
            button.onClick.RemoveAllListeners();
            
            if (button.name != "RollButton") 
            {
                button.onClick.AddListener(() =>
                {
                AudioManager.Instance.PlaySFX("Press");
                });
            }
        }
    }
}

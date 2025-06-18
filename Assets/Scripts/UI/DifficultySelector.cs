using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
    public void SetEasy() => GameManager.Instance.SetDifficulty(Difficulty.Easy);
    public void SetMedium() => GameManager.Instance.SetDifficulty(Difficulty.Medium);
    public void SetHard() => GameManager.Instance.SetDifficulty(Difficulty.Hard);
}

using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Enemies/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public int easyHealth = 10;
    public int mediumHealth = 20;
    public int hardHealth = 30;
    public int easyScore = 10;
    public int mediumScore = 20;
    public int hardScore = 30;

    public int GetHealthByDifficulty(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => easyHealth,
            Difficulty.Medium => mediumHealth,
            Difficulty.Hard => hardHealth,
            _ => easyHealth
        };
    }

    public int GetScoreByDifficulty(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => easyScore,
            Difficulty.Medium => mediumScore,
            Difficulty.Hard => hardScore,
            _ => easyScore
        };
    }
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

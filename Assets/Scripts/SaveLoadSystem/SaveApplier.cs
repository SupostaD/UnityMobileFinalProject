using UnityEngine;

public class SaveApplier : MonoBehaviour
{
    void Start()
    {
        var data = GameManager.Instance.PendingLoadData;
        if (data == null) return;

        GameManager.Instance.SetPlayerName(data.playerName);
        GameManager.Instance.SetDifficulty((Difficulty)System.Enum.Parse(typeof(Difficulty), data.difficulty));
        GameManager.Instance.SetControlScheme((ControlScheme)System.Enum.Parse(typeof(ControlScheme), data.controlScheme));
        GameManager.Instance.SetScore(data.score);
        GameManager.Instance.SetElapsedTime(data.elapsedTime);

        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = data.playerPosition;
        player.GetComponent<Health>().SetHealth(data.playerHP);

        foreach (var enemyData in data.enemies)
        {
            Enemy enemy = Enemy.FindById(enemyData.EnemyId);
            if (enemy != null)
            {
                enemy.ApplySaveData(enemyData);
            }
        }

        GameManager.Instance.SetPendingLoadData(null);
        Time.timeScale = 1f;
    }
}

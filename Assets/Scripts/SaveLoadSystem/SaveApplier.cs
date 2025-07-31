using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveApplier : MonoBehaviour
{
    public static SaveApplier Instance;

    public GameObject bulletFromPlayerPrefab;
    public GameObject bulletFromEnemyPrefab;
    public GameObject enemyPrefab;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameManager.Instance.PendingLoadData != null)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            StartCoroutine(ApplyDataDelayed());
        }
    }

    private IEnumerator ApplyDataDelayed()
    {
        yield return new WaitForSeconds(0.8f);

        var data = GameManager.Instance.PendingLoadData;
        if (data == null) yield break;

        Debug.Log("Applying saved data...");

        GameManager.Instance.SetPlayerName(data.playerName);
        GameManager.Instance.SetDifficulty((Difficulty)System.Enum.Parse(typeof(Difficulty), data.difficulty));
        GameManager.Instance.SetControlScheme((ControlScheme)System.Enum.Parse(typeof(ControlScheme), data.controlScheme));
        GameManager.Instance.SetScore(data.score);
        GameManager.Instance.SetElapsedTime(data.elapsedTime);

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = data.playerPosition;
            player.transform.rotation = data.playerRotation;
            Debug.Break();
            
            player.GetComponent<Health>().SetHealth(data.playerHP);

            var shooter = player.GetComponent<AutoShooter>();
            if (shooter != null)
            {
                shooter.SetCooldown(data.weaponCooldown);
                shooter.SetBulletCount(data.bulletsInMagazine);
            }

            var roll = player.GetComponent<PlayerRoll>();
            if (roll != null)
            {
                roll.SetCooldownRoll(data.rollCooldown);
            }

            var grenadeThrower = player.GetComponent<GrenadeThrower>();
            if (grenadeThrower != null)
            {
                grenadeThrower.SetCooldownGrenade(data.grenadeCooldown);

                if (data.hasActiveGrenade)
                {
                    var grenadeData = data.activeGrenade;
                    GameObject grenadeObj = Instantiate(grenadeThrower.GrenadePrefab, grenadeData.position, Quaternion.identity);
                    Rigidbody rb = grenadeObj.GetComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.linearVelocity = grenadeData.velocity;

                    var grenade = grenadeObj.GetComponent<Grenade>();
                    grenade.ApplySaveData(grenadeData);
                }
            }
        }

        var spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
            spawner.DisableSpawningFromSave();

        foreach (var enemyData in data.enemies)
        {
            GameObject enemyObj = Instantiate(enemyPrefab, enemyData.Position, enemyData.Rotation);
            var enemy = enemyObj.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ApplySaveData(enemyData);
            }
        }

        foreach (var bulletData in data.bulletsFromPlayer)
        {
            GameObject bulletObj = Instantiate(bulletFromPlayerPrefab, bulletData.position, bulletData.rotation);
            var bullet = bulletObj.GetComponent<BulletMovement>();
            bullet?.ApplySaveData(bulletData);
        }

        foreach (var bulletData in data.bulletsFromEnemy)
        {
            GameObject bulletObj = Instantiate(bulletFromEnemyPrefab, bulletData.position, bulletData.rotation);
            var bullet = bulletObj.GetComponent<EnemyBullet>();
            bullet?.ApplySaveData(bulletData);
        }

        GameManager.Instance.SetPendingLoadData(null);
        Time.timeScale = 1f;
    }
}

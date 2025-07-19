using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveApplier : MonoBehaviour
{
    public GameObject bulletFromPlayerPrefab;
    public GameObject bulletFromEnemyPrefab;
    public GameObject enemyPrefab;
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    /*
    void Awake()
    */
    {
        var data = GameManager.Instance.PendingLoadData;
        if (data == null) return;

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
            player.GetComponent<Health>().SetHealth(data.playerHP);

            var shooter = player.GetComponent<AutoShooter>();
            if (shooter != null)
            {
                shooter.SetCooldown(data.weaponCooldown);
                shooter.SetBulletCount(data.bulletsInMagazine);
                Debug.Log("Set cooldown bullets and set bullet count from save!");
            }

            var roll = player.GetComponent<PlayerRoll>();
            if (roll != null)
            {
                roll.SetCooldownRoll(data.rollCooldown);
                Debug.Log("Set cooldown roll from save!");
            }

            var grenadeThrower = player.GetComponent<GrenadeThrower>();
            if (grenadeThrower != null)
            {
                grenadeThrower.SetCooldownGrenade(data.grenadeCooldown);
                Debug.Log("Set cooldown grenade thrower from save!");
            }

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

                Debug.Log("Spawning grenade from save!");
            }
        }

        var spawner = FindObjectOfType<EnemySpawner>();
        spawner.suppressSpawning = true;

        foreach (var enemyData in data.enemies)
        {
            GameObject enemyObj = Instantiate(enemyPrefab, enemyData.Position, enemyData.Rotation);
            Enemy enemy = enemyObj.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.ApplySaveData(enemyData);
                Debug.Log($"Spawned enemy");
            }
        }

        spawner.suppressSpawning = false;
        
        foreach (var bulletData in data.bulletsFromPlayer)
        {
            GameObject bulletObj = Instantiate(bulletFromPlayerPrefab, bulletData.position, bulletData.rotation);
            var bullet = bulletObj.GetComponent<BulletMovement>();
            bullet?.ApplySaveData(bulletData);
            Debug.Log("Spawned bullet from player from save!");
        }

        foreach (var bulletData in data.bulletsFromEnemy)
        {
            GameObject bulletObj = Instantiate(bulletFromEnemyPrefab, bulletData.position, bulletData.rotation);
            var bullet = bulletObj.GetComponent<EnemyBullet>();
            bullet?.ApplySaveData(bulletData);
            Debug.Log("Spawned bullet from enemy from save!");
        }

        GameManager.Instance.SetPendingLoadData(null);
        Time.timeScale = 1f;
    }
}

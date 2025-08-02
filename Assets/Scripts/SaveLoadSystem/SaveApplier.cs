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
    }

    public IEnumerator ApplyDataDelayed()
    {
        var data = GameManager.Instance.PendingLoadData;
        if (data == null)
        {
            Debug.LogWarning("No data to apply.");
            yield break;
        }

        // Ждем появления игрока
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("2 " + player.name + player.transform.position);

        yield return new WaitForSeconds(1.3f); // подстраховка, лучше чем 0.8

        ApplyData();

        GameManager.Instance.SetPendingLoadData(null);
    }

    private void ApplyData()
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
            Debug.Log("3 " + player.name + player.transform.position);
            //Debug.Break();

            player.GetComponent<Health>().SetHealth(data.playerHP);

            var shooter = player.GetComponent<AutoShooter>();
            if (shooter != null)
            {
                shooter.SetCooldown(data.weaponCooldown);
                shooter.SetBulletCount(data.bulletsInMagazine);
            }

            var roll = player.GetComponent<PlayerRoll>();
            if (roll != null)
                roll.SetCooldownRoll(data.rollCooldown);

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
            spawner.waitForLoad = false;

        foreach (var enemyData in data.enemies)
        {
            GameObject enemyObj = Instantiate(enemyPrefab, enemyData.Position, enemyData.Rotation);
            var enemy = enemyObj.GetComponent<Enemy>();
            enemy?.ApplySaveData(enemyData);
        }

        foreach (var bulletData in data.bulletsFromPlayer)
        {
            GameObject bulletObj = Instantiate(bulletFromPlayerPrefab, bulletData.position, bulletData.rotation);
            bulletObj.GetComponent<BulletMovement>()?.ApplySaveData(bulletData);
        }

        foreach (var bulletData in data.bulletsFromEnemy)
        {
            GameObject bulletObj = Instantiate(bulletFromEnemyPrefab, bulletData.position, bulletData.rotation);
            bulletObj.GetComponent<EnemyBullet>()?.ApplySaveData(bulletData);
        }
    }
}

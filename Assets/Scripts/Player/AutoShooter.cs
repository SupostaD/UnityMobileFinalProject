using UnityEngine;
using UnityEngine.UI;

public class AutoShooter : MonoBehaviour
{
    [Header("Combat")]
    public Transform FirePoint;
    public float BulletSpeed = 10f;
    public float FireRate = 1f;
    public float MinShootRadius = 5.1f;
    public LayerMask EnemyLayer;
    public Collider shooterCollider;
    public Animator animator;

    [Header("Reloading")]
    public int MaxBullets = 5;
    public float ReloadDuration = 2f;
    public Image ReloadFillImage;
    public Sprite[] MagazineSprites; // Index 0 = empty, 5 = full

    [Header("Refs")]
    public CircleDrawerDynamicAim AimCircle;
    public PlayerRoll PlayerRoll;
    
    public float GetCooldown() => fireCooldown;
    public int GetBulletCount() => currentBullets;
    public void SetCooldown(float value) => fireCooldown = value;
    public void SetBulletCount(int value) => currentBullets = value;

    private float fireCooldown;
    private int currentBullets;
    private bool isReloading;
    private float reloadTimer;

    void Start()
    {
        currentBullets = MaxBullets;
        UpdateMagazineSprite(); 
        ReloadFillImage.fillAmount = 1f;
        if (PlayerRoll == null)
            PlayerRoll = GetComponent<PlayerRoll>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (isReloading)
        {
            reloadTimer -= Time.deltaTime;
            ReloadFillImage.fillAmount = 1f - (reloadTimer / ReloadDuration);

            if (reloadTimer <= 0f)
            {
                isReloading = false;
                currentBullets = MaxBullets;
                UpdateMagazineSprite();
                ReloadFillImage.fillAmount = 1f;
            }
            return;
        }

        if (PlayerRoll.IsRolling())
            return;

        fireCooldown -= Time.deltaTime;

        float radius = AimCircle != null ? AimCircle.currentRadius : 5f;
        if (radius <= MinShootRadius)
            return;

        Collider[] enemies = Physics.OverlapSphere(transform.position, radius * 2, EnemyLayer);
        if (enemies.Length > 0 && fireCooldown <= 0f && currentBullets > 0)
        {
            Transform target = FindClosestEnemy(enemies);
            if (target != null)
            {
                Shoot(target);
            }
        }
        
        if (currentBullets <= 0)
        {
            AudioManager.Instance.PlaySFX("Reload");
            StartReload();
        }
    }

    void Shoot(Transform target)
    {
        Vector3 dir = (target.position - FirePoint.position).normalized;

        Vector3 lookDir = target.position - transform.position;
        lookDir.y = 0f;

        if (lookDir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(lookDir);

        RaycastHit hit;
        float distance = Vector3.Distance(FirePoint.position, target.position);

        int mask = ~LayerMask.GetMask("IgnoreRaycast");
        if (Physics.Raycast(FirePoint.position, dir, out hit, distance, mask))
        {
            if (!hit.transform.CompareTag("Enemy"))
                return;
        }

        GameObject bullet = PlayerBulletPool.Instance.GetBullet(FirePoint.position, Quaternion.LookRotation(dir));

        Collider bulletCollider = bullet.GetComponent<Collider>();
        if (bulletCollider != null && shooterCollider != null)
            Physics.IgnoreCollision(bulletCollider, shooterCollider);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = dir * BulletSpeed;
        
        AudioManager.Instance.PlaySFX("Shoot");
        
        animator.SetBool("isShooting", true);
        
        fireCooldown = 1f / FireRate;
        currentBullets--;
        UpdateMagazineSprite();
    }

    Transform FindClosestEnemy(Collider[] enemies)
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var col in enemies)
        {
            float dist = (col.transform.position - transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closest = col.transform;
            }
        }
        return closest;
    }

    void StartReload()
    {
        isReloading = true;
        reloadTimer = ReloadDuration;
        if (ReloadFillImage != null)
            ReloadFillImage.fillAmount = 0f;
    }

    void UpdateMagazineSprite()
    {
        int index = Mathf.Clamp(currentBullets, 0, MagazineSprites.Length - 1);
        ReloadFillImage.sprite = MagazineSprites[index];
    }
}

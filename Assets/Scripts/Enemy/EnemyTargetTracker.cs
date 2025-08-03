using UnityEngine;

public class EnemyTargetTracker  : MonoBehaviour
{
    public string playerTag = "Player";
    public EnemyShooter shooter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            shooter.SetTarget(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            shooter.ClearTarget();
        }
    }
}

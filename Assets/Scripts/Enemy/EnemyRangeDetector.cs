using UnityEngine;

public enum DetectionType { Vision, Shoot }

public class EnemyRangeDetector : MonoBehaviour
{
    public DetectionType type;
    public string playerTag = "Player";

    private EnemyAI ai;

    private void Awake()
    {
        ai = GetComponentInParent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (type == DetectionType.Vision)
            ai.OnPlayerEnteredVision(other.transform);
        else if (type == DetectionType.Shoot)
            ai.OnPlayerEnteredShootRange();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (type == DetectionType.Vision)
            ai.OnPlayerExitedVision();
        else if (type == DetectionType.Shoot)
            ai.OnPlayerExitedShootRange();
    }
}

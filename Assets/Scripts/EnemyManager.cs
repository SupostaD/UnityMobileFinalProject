using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private bool isActivated = false;

    void OnBecameVisible()
    {
        if (!isActivated)
        {
            isActivated = true;
            gameObject.SetActive(true);
        }
    }

    void OnBecameInvisible()
    {
        if (!isActivated)
        {
            gameObject.SetActive(false);
        }
    }

    public void Initialize()
    {
        isActivated = false;
        gameObject.SetActive(false);
    }
}

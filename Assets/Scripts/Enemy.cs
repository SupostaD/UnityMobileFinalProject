using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isActivated = false;

    public Renderer enemyRenderer;
    public Camera mainCamera;

    void Start()
    {
        SetInactiveState();
    }

    void Update()
    {
        bool isVisible = IsVisibleToCamera(mainCamera);

        if (isVisible && !isActivated)
            Activate();
        else if (!isVisible && isActivated)
            SetInactiveState();
    }

    bool IsVisibleToCamera(Camera cam)
    {
        if (enemyRenderer == null) 
            return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, enemyRenderer.bounds);
    }

    public void SetInactiveState()
    {
        isActivated = false;
        if (enemyRenderer != null)
            enemyRenderer.enabled = false;
    }

    public void Activate()
    {
        isActivated = true;
        if (enemyRenderer != null)
            enemyRenderer.enabled = true;
    }
}

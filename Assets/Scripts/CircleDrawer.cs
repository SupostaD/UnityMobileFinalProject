using UnityEngine;
using UnityEngine.InputSystem;

public class CircleDrawer : MonoBehaviour
{
    public Transform Player;
    public float Radius = 5f;
    public int Segments = 100;
    private LineRenderer lineRenderer;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = Segments;

        UpdateCircle();
    }

    void Update()
    {
        UpdateCircle();

        transform.position = new Vector3(Player.position.x, transform.position.y, Player.position.z);
    }

    void UpdateCircle()
    {
        for (int i = 0; i < Segments; i++)
        {
            float angle = ((float)i / Segments) * Mathf.PI * 2f;
            float x = Mathf.Cos(angle) * Radius;
            float z = Mathf.Sin(angle) * Radius;
            lineRenderer.SetPosition(i, new Vector3(x, 0f, z));
        }
    }
}

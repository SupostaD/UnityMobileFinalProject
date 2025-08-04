using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 10f, -10f);
    public Vector3 rotationAngles = new Vector3(45f, 0f, 0f);

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(rotationAngles);
    }
}

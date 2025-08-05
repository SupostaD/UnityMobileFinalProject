using UnityEngine;

public class RippleSpawner : MonoBehaviour
{
    public static RippleSpawner Instance { get; private set; }

    public GameObject ripplePrefab;
    public Canvas canvas;

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

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            SpawnRippleAtPosition(Input.GetTouch(0).position);
        }
    }

    public void SpawnRippleAtPosition(Vector2 screenPosition)
    {
        if (ripplePrefab == null || canvas == null)
        {
            return;
        }

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.worldCamera,
            out localPos
        );

        GameObject ripple = Instantiate(ripplePrefab, canvas.transform);
        ripple.GetComponent<RectTransform>().anchoredPosition = localPos;
    }
}

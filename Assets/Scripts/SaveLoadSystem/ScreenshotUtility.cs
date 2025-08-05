using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ScreenshotUtility : MonoBehaviour
{
    public static ScreenshotUtility Instance;

    [SerializeField] private Camera screenshotCamera;
    [SerializeField] private int width = 1920;
    [SerializeField] private int height = 1080;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Texture2D Capture()
    {
        RenderTexture rt = new RenderTexture(width, height, 24);
        screenshotCamera.targetTexture = rt;

        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenshotCamera.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        return screenshot;
    }

    public void SaveTextureToPNG(Texture2D tex, string fileName)
    {
        byte[] bytes = tex.EncodeToPNG();
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(path, bytes);
    }
}

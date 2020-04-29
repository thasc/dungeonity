using System;
using System.IO;
using UnityEngine;

public class Photographer : MonoBehaviour
{
    private Camera m_Camera;

    private void Awake()
    {
        m_Camera = gameObject.AddComponent<Camera>();
        // set m_Camera.aspect to determine its aspect ratio, and orthographicSize for size
        m_Camera.tag = "PhotographerCamera";
        m_Camera.orthographic = true;
        m_Camera.forceIntoRenderTexture = true;
        m_Camera.enabled = false;
    }
    
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            Snap();  
        }
    }

    private void Snap()
    {
        var mainCamera = Camera.main;
        mainCamera.enabled = false;
        m_Camera.enabled = true;
        
        // todo: adjust camera to capture the whole map to an adequately sized render texture 
    
        var image = RenderCameraView(m_Camera);
        
        // todo: crop image to the correct size for export
        
        var bytes = image.EncodeToPNG();
        Destroy(image);

        var captureFolder = Path.Combine(Application.dataPath, "Captures");
        Directory.CreateDirectory(captureFolder);
        File.WriteAllBytes(Path.Combine(captureFolder, DateTime.UtcNow.Ticks + ".png"), bytes);

        m_Camera.enabled = false;
        mainCamera.enabled = true;
    }

    private static Texture2D RenderCameraView(Camera camera)
    {
        var targetTexture = RenderTexture.GetTemporary(1024, 1024);
        var suspendedRenderTexture = RenderTexture.active;
        RenderTexture.active = camera.targetTexture = targetTexture;
        
        camera.Render();
        var image = new Texture2D(targetTexture.width, targetTexture.height);
        image.ReadPixels(new Rect(0, 0, targetTexture.width, targetTexture.height), 0, 0);
        image.Apply();

        camera.targetTexture = null;
        RenderTexture.active = suspendedRenderTexture;
        RenderTexture.ReleaseTemporary(targetTexture);
        
        return image;
    }
}


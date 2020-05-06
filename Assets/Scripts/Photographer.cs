using System;
using System.IO;
using UnityEngine;

public class Photographer : MonoBehaviour
{
    private Camera m_Camera;
    private DungeonStateManager m_DungeonStateManager;

    private void Awake()
    {
        m_DungeonStateManager = GameObject.FindWithTag("DungeonStateManager")
                                    ?.GetComponent<DungeonStateManager>() ??
                                throw new ArgumentNullException(nameof(m_DungeonStateManager));
        
        m_Camera = gameObject.GetComponent<Camera>();
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
        var start = DateTime.UtcNow;
        
        var image = RenderCameraView();
        
        var bytes = image.EncodeToPNG();
        Destroy(image);

        var captureFolder = Path.Combine(Application.dataPath, "Captures");
        Directory.CreateDirectory(captureFolder);
        File.WriteAllBytes(Path.Combine(captureFolder, DateTime.UtcNow.Ticks + ".png"), bytes);
        
        Debug.Log($"Wrote dungeon out in {(DateTime.UtcNow - start).Milliseconds}ms");
    }

    private Texture2D RenderCameraView()
    {
        const int pixelsPerTile = 64;
        
        var dungeonWidthInPixels = m_DungeonStateManager.Width * pixelsPerTile;
        var dungeonHeightInPixels = m_DungeonStateManager.Height * pixelsPerTile;
        
        var mainCamera = Camera.main;
        mainCamera.enabled = false;
        m_Camera.enabled = true;
        
        var suspendedRenderTexture = RenderTexture.active;
        //var targetTexture = new RenderTexture(1024, 1024, 16);
        //targetTexture.Create();
        var targetTexture = RenderTexture.GetTemporary(dungeonWidthInPixels, dungeonHeightInPixels);
        RenderTexture.active = m_Camera.targetTexture = targetTexture;
        
        m_Camera.transform.position = new Vector3(m_DungeonStateManager.Width/2f, m_DungeonStateManager.Height/2f, -10);
        m_Camera.aspect = (dungeonWidthInPixels * 1f) / dungeonHeightInPixels;
        m_Camera.orthographicSize = m_DungeonStateManager.Height / 2f;
        m_Camera.Render();
        var image = new Texture2D(targetTexture.width, targetTexture.height);
        image.ReadPixels(new Rect(0, 0, targetTexture.width, targetTexture.height), 0, 0);
        image.Apply();

        m_Camera.targetTexture = null;    
        RenderTexture.active = suspendedRenderTexture;
        //targetTexture.Release();
        RenderTexture.ReleaseTemporary(targetTexture);
        
        m_Camera.enabled = false;
        mainCamera.enabled = true;
        
        return image;
    }
}


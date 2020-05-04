using UnityEngine;

public class DungeonStateManager : MonoBehaviour
{
    public int Width;
    public int Height;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}

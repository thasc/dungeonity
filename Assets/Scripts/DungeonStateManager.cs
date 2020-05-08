using UnityEngine;

public class DungeonStateManager : MonoBehaviour
{
    public int Width;
    public int Height;
    public string DonjonTsvFilePath;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}

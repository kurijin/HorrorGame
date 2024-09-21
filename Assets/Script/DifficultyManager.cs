using UnityEngine;

/// <summary>
/// ゲーム内の変動する怖さ(以降難易度)について常に保持しておくもの
/// </summary>
public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }
    //ゲームの難易度 0:こわくない 1:ふつう　2:こわい
    [SerializeField,Header("ゲームの難易度")] public int gameLevel = 0;
    public bool isLevelLocked = false;
    public int Level
    {
        get { return gameLevel; }
        set
        {
            if (!isLevelLocked)  
            {
                gameLevel = value;
            }
            else
            {
                Debug.LogWarning("Level is locked");
            }
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); 
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Variables to track high score and game mode
    public int HighScore { get; private set; }
    public int GameMode { get; private set; } // 0:Menu 1:OneDuck 2:TwoDuck 3:ClayPigeon

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize high score and game mode
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        //GameMode = "Classic"; // Default game mode, confirm name
    }

    // Method to update high score
    public void UpdateHighScore(int newScore)
    {
        if (newScore > HighScore)
        {
            HighScore = newScore;
            PlayerPrefs.SetInt("HighScore", HighScore);
        }
    }

    // Method to switch game modes
    public void SetGameMode(int mode)
    {
        if (mode >= 0 && mode <= 3)
        {
            GameMode = mode;
        }
        else
        {
            Debug.LogWarning("Invalid game mode selected.");
        }
    }
}

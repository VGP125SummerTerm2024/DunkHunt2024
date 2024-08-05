using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _Instance;
    public static GameManager Instance => _Instance;

    // Variables to track high score and game mode
    int highScore;

    public int GameMode // 0:Menu 1:OneDuck 2:TwoDuck 3:ClayPigeon
    { 
        get => _GameMode; 
        set => _GameMode = value;
    }

    private int _GameMode;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize high score and game mode
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        _GameMode = 1; //Set gamemode to default 1 duck mode
    }

    // Method to update high score
    public void UpdateHighScore(int newScore)
    {
        if (newScore > highScore)
        {
            highScore = newScore;
            PlayerPrefs.SetInt("HighScore", highScore);
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

    public void pause()
    {
        Time.timeScale = 0f;
    }

    public void unPause()
    {
        Time.timeScale = 1.0f;
    }
}

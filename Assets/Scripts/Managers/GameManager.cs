using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Variables to track high score and game mode
    int highScore;

    public int GameMode; // 0:Menu 1:OneDuck 2:TwoDuck 3:ClayPigeon

    private int _GameMode;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

        //HighScore = PlayerPrefs.GetInt("HighScore", 0);
        //GameMode = "Classic"; // Default game mode, confirm name
        //LoadMainMenu();

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
        if (mode >= 1 && mode <= 3)
        {
            GameMode = mode;
            //LoadSceneForGameMode();
        }
        else
        {
            Debug.LogWarning("Invalid game mode selected.");
        }
    }


    public void LoadMainMenu()
    {
        //SceneManager.LoadScene("IPM Main Menu");
    }

    //Method to load scenes based on game mode - commenting out, Alexa is working on this
    //private void LoadSceneForGameMode()
    //{
        //switch(GameMode)
        //{
           // case 0:
                //LoadMainMenu();
               // break;
           // case 1:
               // SceneManager.LoadScene("1DuckMode");
               // break;
           //case 2:
                //SceneManager.LoadScene("2DuckMode");
                //break;
            //case 3:
                //SceneManager.LoadScene("ClayPigeonScene");
                //break;
       // }

}

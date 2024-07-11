using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance => _instance;

    public enum DebugModes
    {
        Debug = 0, //use this in development
        Info = 1, //use this or higher in test/release branches
        Warning = 2,
        Error = 3, 
        Critical = 4 //use in production/final release versions
    }

    [SerializeField] private DebugModes debugMode = DebugModes.Debug; //default to debug mode for now, you may wish to change this later
    
    [SerializeField] public int startingAmmo = 3;
    [SerializeField] public int currentAmmo;

    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private PlayerController playerPrefab; //update to input/mouse controller if needed
    [HideInInspector] public PlayerController PlayerInstance => _playerInstance;

    PlayerController _playerInstance = null;

    private int currentLevel = 0; 

    public enum GameScenes //yup, we have to hard code this to the scenes in the build index - I'm sure there's a better way
    {
        title = 0,
        grass = 1,
        gameOver = 2,
    }

    [SerializeField] private GameScenes currentScene = GameScenes.title;

    [SerializeField] private GameScenes gameOverSceneId = GameScenes.gameOver;

    //Set Up Events - modify as needed
    public delegate void GameStart();
    public event GameStart GameStartedEvent;
    public delegate void GameOver();
    public event GameOver GameOverEvent;
    public delegate void GameWon();
    public event GameWon GameWonEvent;
    public delegate void PauseGame();
    public event PauseGame PauseGameEvent;
    public delegate void ResumeGame();
    public event ResumeGame ResumeGameEvent;

    public delegate void LevelStart();
    public event LevelStart LevelStartEvent;
    public delegate void LevelComplete();
    public event LevelComplete LevelCompleteEvent;

    public delegate void PlayerReady();
    public event PlayerReady PlayerReadyEvent;
    public delegate void PlayerShoot();
    public event PlayerShoot PlayerShootEvent;
    //listen if tracking stats such as how many times a player shoots, could later use total to calculate shoot % if desired and possibly other behaviour

    public delegate void DuckMissed();
    public event DuckMissed DuckMissedEvent;
    //broadcast from wherever checks the collision between the bullet and the duck, if bullet missed
    //listen in dog to trigger laughing sound and animation

    public delegate void DuckShot();
    public event DuckShot DuckShotEvent;
    //broadcast from wherever checks the collision between the bullet and the duck
    //listen for DuckShotEvent in score tracking to display and update score,
    //listen in duck script to play duck death animation and start fall sequence,
    

    public delegate void DuckDead();
    public event DuckDead DuckDeadEvent;
    //broadcast from duck when it has finished falling to the ground and is ready to be erased if there is any follow-up to perform

        
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.buildIndex == (int)GameScenes.grass)
        {
            InitLevel(); 

        }
    }

    public void InitLevel() //TODO: probably want to add an `int level` to InitLevel() since all our levels will likely use the same scene
    {
        //TODO: call any scripts to set up the level 

        //Add any listeners to events
        GameManager.Instance.GameOverEvent += GameOverScene;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene(int index)
    {

        Debug.Log("loading scene " + index);
        currentAmmo = startingAmmo;
        SceneManager.LoadScene(index);
        if (index == (int)GameScenes.grass)
        {
            InitLevel();
        }

    }
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
        InitLevel();
    }

    public void MergeScene(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Additive);
    }

    public void UnloadScene(int index)
    {
        SceneManager.UnloadSceneAsync(index);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void StartGame(int lives)
    {
        if (GameManager.Instance.GameStartedEvent != null)
        {
            if (debugMode == DebugModes.Debug)
                Debug.Log("Start game");  

            GameManager.Instance.GameStartedEvent();
        }
        else
        {
            if (debugMode <= DebugModes.Error)
                Debug.Log("Unable to start game");
        }
    }

    public void GameOverSequence()
    {
        if (debugMode <= DebugModes.Debug)
            Debug.Log("Start Game Over Sequence");

        if (GameManager.Instance.GameOverEvent != null)
        {
            GameManager.Instance.GameOverEvent();
        }
    }

    public void GameOverScene()
    {
        if (debugMode <= DebugModes.Debug)
            Debug.Log("Load Game Over Scene");

        LoadScene((int)GameScenes.gameOver);//even hard coded this scene is no longer being loaded
        
    }

    //Initiate GameWon
    public void GameWonSequence()
    {
        if (debugMode <= DebugModes.Debug)
            Debug.Log("Start Game Won Sequence");

        if (GameManager.Instance.GameWonEvent != null)
        {
            GameManager.Instance.GameWonEvent();
        }
    }

    //Pause Game
    public void GamePaused()
    {
        if (debugMode <= DebugModes.Debug)
            Debug.Log("Pause Game");

        if (GameManager.Instance.PauseGameEvent != null)
        {
            GameManager.Instance.PauseGameEvent();
        }
    }

    //Resume Game
    public void GameResumed()
    {
        if (debugMode <= DebugModes.Debug)
            Debug.Log("Resume Game");

        if (GameManager.Instance.ResumeGameEvent != null)
        {
            GameManager.Instance.ResumeGameEvent();
        }
    }

    public void UpdateDuckMissed()
    {
        if (debugMode <= DebugModes.Debug)
            Debug.Log("Duck Missed");
        if (GameManager.Instance.DuckMissedEvent != null)
        {
            GameManager.Instance.DuckMissedEvent();
        }
    }

    public void UpdateDuckShot()
    {
        if (debugMode <= DebugModes.Debug)
            Debug.Log("Duck Shot");
        if (GameManager.Instance.DuckShotEvent != null)
        {
            GameManager.Instance.DuckShotEvent();
        }
    }

    public void UpdateDuckDead()
    {
        if (GameManager.Instance.DuckDeadEvent != null)
        {
            GameManager.Instance.DuckDeadEvent();
        }
    }    

    public void UpdateLevelComplete()
    {
        if (GameManager.Instance.LevelCompleteEvent != null)
        {
            LevelCompleteEvent();
        }
    }

    public void UpdateLevelStart()
    {
        if (GameManager.Instance.LevelStartEvent != null)
        {
            GameManager.Instance.LevelStartEvent();
        }
    }

    public void UpdatePlayerShoot()
    {
        if (GameManager.Instance.PlayerShootEvent != null)
        {
            GameManager.Instance.PlayerShootEvent();
        }
        else
        {
            Debug.LogError("What is going on with #lab7?");
        }

    }

    public void UpdatePlayerReady(int lives)
    {
        if (PlayerReadyEvent != null)
        {
            GameManager.Instance.PlayerReadyEvent();
        }
        else
        {
            Debug.LogError("What is going on with #lab7?");
        }

    }

    public void Respawn()
    {
        //_playerInstance.transform.position = currentCheckpoint.position;
    }

    public void SpawnPlayer(Transform spawnLocation) //TODO: update for dog or ducks or both
    {

        _playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
        //currentCheckpoint = spawnLocation;
    }

    public void DestroyPlayer() //TODO: update or add additional function for dog or ducks? 
    {
        Destroy(_playerInstance);
        //        Destroy(playerController);
    }
}

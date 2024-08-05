using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController: MonoBehaviour
{
    static SceneController _instance;
    public static SceneController Instance => _instance;

    [Header("Buttons")]
    public Button StartButton;
    public Button MenuButton;
    public Button QuitButton;

    [Header("Scene Names")]
    public string LevelOne;
    public string Menu;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void Start()
    {
        if (QuitButton)
        {
            
            QuitButton.onClick.AddListener(Quit);
        }

        if (StartButton)
        {
            StartButton.onClick.AddListener(() => LoadScene(LevelOne)); 
        }

        if (MenuButton)
        {
            MenuButton.onClick.AddListener(() => LoadScene(Menu)); 
        }


    }
    void Quit()
    {
        Debug.Log("Quitting Game...");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    void LoadScene(string SceneName)
    {
        Debug.Log("Loading Scene");
        SceneManager.LoadScene(SceneName);
    }
}

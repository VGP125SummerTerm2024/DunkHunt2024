using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// This is the start of the singleton pattern logic.
    /// </summary>
    private static ScoreManager instance;

    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Try to find an existing instance in the scene
                instance = FindObjectOfType<ScoreManager>();

                // If no instance is found, create a new one
                if (instance == null)
                {
                    GameObject singleton = new GameObject(typeof(ScoreManager).ToString());
                    instance = singleton.AddComponent<ScoreManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return instance;
        }
    }

    private ScoreManager() {}

    // For testing and confirmation.
    public void DisplayMessage()
    {
        Debug.Log("Singleton Instance Called for ScoreManager");
    }
    /// <summary>
    /// This is the end of the singleton pattern logic.
    /// </summary>

    public Text score;
    public Text round;

    public int currentRound = 1;
    public int scoreValue;

    private int duckMultiplier;
    private int perfectMultiplier;

    private int blackBase = 500;
    private int blueBase = 1000;
    private int redBase = 1500;
    private int perfectBase = 10000;

    public Button B1;
    public Button B2;
    public Button B3;
    public Button B4;
    public Button AR;
    public Button MR;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Doing this just so that we're not displaying null value.
        score.text = "000000"; 
    }

    void Update()
    {
        // This does not need to be ran every frame, but until I can marry this and the round counter, I will leave it here.
        // Once I have access to the round counter, then this method will be called once per round.
        RoundMultiplier();
        round.text = currentRound.ToString();
    }

    // The method which will be called when the black duck is clicked.
    public void _BlackDuck() 
    {
        if (currentRound <= 5)
        {
            scoreValue += blackBase;

            Debug.Log("Score that was just added = " + blackBase);
        }
        else
        {
            scoreValue += blackBase + (blackBase * duckMultiplier / 100);

            Debug.Log("Score that was just added = " + (blackBase + (blackBase * duckMultiplier / 100)));
        }

        ResetButtonVisual(B1);

        ScoreFormatter();
    }

    // The method which will be called when the blue duck is clicked.
    public void _BlueDuck()
    {
        if (currentRound <= 5)
        {
            scoreValue += blueBase;

            Debug.Log("Score that was just added = " + blueBase);
        }
        else
        {
            scoreValue += blueBase + (redBase * duckMultiplier / 100);

            Debug.Log("Score that was just added = " + (blueBase + (blueBase * duckMultiplier / 100)));
        }

        ResetButtonVisual(B2);
        
        ScoreFormatter();
    }

    // The method which will be called when the red duck is clicked.
    public void _RedDuck() 
    {
        if (currentRound <= 5)
        {
            scoreValue += redBase;

            Debug.Log("Score that was just added = " + redBase);
        }
        else
        {
            scoreValue += redBase + (redBase * duckMultiplier / 100);

            Debug.Log("Score that was just added = " + (redBase + (redBase * duckMultiplier / 100)));
        }

        ResetButtonVisual(B3);

        ScoreFormatter();
    }

    public void _PerfectScore()
    {
        if (currentRound <= 10)
        {
            scoreValue += perfectBase;

            Debug.Log("Score that was just added = " + perfectBase);
        }
        else if (currentRound >= 11 && currentRound <= 99)
        {
            scoreValue += perfectBase + (perfectBase * perfectMultiplier / 100);

            Debug.Log("Score that was just added = " + (perfectBase + (perfectBase * perfectMultiplier / 100)));
        }

        ResetButtonVisual(B4);

        ScoreFormatter();
    }

    public void AddRound()
    {
        currentRound++;

        if (currentRound >= 99)
        {
            currentRound = 99;
        }

        ResetButtonVisual(AR);
    }

    public void MinusRound()
    {
        currentRound--;

        if (currentRound <= 0)
        {
            currentRound = 1;
        }

        ResetButtonVisual(MR);
    }

    // The method which will reset the button visual. This precents the "Selected" colour from being applied to the button.
    private void ResetButtonVisual(Button button)
    {
        button.interactable = false;
        button.interactable = true;
    }

    //I know this is over complicated, but for now it does what I need it to do.
    private void RoundMultiplier()
    {

        if (currentRound >= 6 && currentRound <= 10)
        {
            duckMultiplier = 60;
        }
        else if (currentRound >= 11 && currentRound <= 15)
        {
            duckMultiplier = 100;
            perfectMultiplier = 50;
        }
        else if (currentRound >= 16 && currentRound <= 20)
        {
            perfectMultiplier = 100;
        }
        else if (currentRound >= 21)
        {
            perfectMultiplier = 200;
        }
    }

    // The method which will format the score text. Duck hunt uses the old style of scoring, so we need to format it to look like that.
    public void ScoreFormatter() 
    {
        if (scoreValue < 1000)
        {
            score.text = "000" + scoreValue.ToString();
        }
        else if (scoreValue < 10000)
        {
            score.text = "00" + scoreValue.ToString();
        }
        else if (scoreValue < 100000)
        {
            score.text = "0" + scoreValue.ToString();
        }
        else if (scoreValue >= 100000)
        {
            score.text = scoreValue.ToString();
        }

    }
}

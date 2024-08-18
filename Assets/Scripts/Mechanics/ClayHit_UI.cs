using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClayHit_UI : MonoBehaviour
{
    private int level;
    private int ducksToHit;  // Number of ducks that need to be hit to pass the level
    private const int ducksPerLevel = 10;
    private int ducksMissed;
    private int hits;
    private int ducksProcessed; // Counter for processed ducks

    public GameObject[] duckImages; // Array of duck images
    public Slider ducksToHitSlider; // Reference to the Slider component

    RoundUI RoundUI;
    ClayRoundManager crm; 
    void Start()
    {
        level = 1;
        ducksToHit = CalculateDucksToHit(level);
        hits = 0;
        ducksMissed = 0;
        ducksProcessed = 0; // Initialize counter
        Debug.Log("Welcome to Duck Hunt!");
        crm = FindAnyObjectByType<ClayRoundManager>();
        RoundUI = FindAnyObjectByType<RoundUI>();
        StartLevel();
        
        
    }

    void Update()
    {
    }

    private void StartLevel()
    {
        hits = 0;
        ducksMissed = 0;
        ducksProcessed = 0; // Reset counter
        UpdateUI();
    }

    public void RegisterHit()
    {
        if (ducksProcessed < ducksPerLevel)
        {
            hits++;
            Debug.Log("Duck hit!");
            duckImages[ducksProcessed].SetActive(true); // Make the duck image visible
            ducksProcessed++;
            if (ducksProcessed >= ducksPerLevel)
            {
                CheckGameState();
            }
        }
    }

    public void RegisterMiss()
    {
        if (ducksProcessed < ducksPerLevel)
        {
            ducksMissed++;
            Debug.Log("Missed!");
            duckImages[ducksProcessed].SetActive(false); // Keep the duck image invisible
            ducksProcessed++;
            if (ducksProcessed >= ducksPerLevel)
            {
                CheckGameState();
            }
        }
    }

    public void CheckGameState()
    {
        if (hits >= ducksToHit)
        {
            Debug.Log($"You hit {hits} ducks! Moving to the next level.");
            if (hits == 10)
            {
                IPMScoreManager.Instance._PerfectScore();
            }
            StartLevel();
            crm._roundCount++;
            crm.round++;
            RoundUI.IncrementRound();
            crm.StartNewRound();
            
        }
        else
        {
            Debug.Log($"You missed {ducksMissed} ducks and hit {hits} ducks. You need to hit {ducksToHit} ducks to pass the level.");
            if (ducksMissed + hits == ducksPerLevel)
            {
                // If the player has processed all ducks but didn't hit enough
                Debug.Log("Not enough ducks hit. Game Over!");
                crm.gameover = true;
                crm.gameOver();
            }
        }
    }

    private void NextLevel()
    {
        level++;
        ducksToHit = CalculateDucksToHit(level);// Increase ducks to hit, max is 9
        StartLevel();
    }

    private void UpdateUI()
    {
        // Reset the duck images to invisible
        foreach (GameObject duckImage in duckImages)
        {
            duckImage.SetActive(false);
        }

        // Update the slider fill amount based on the level
        float fillValue = CalculateFillValue();
        ducksToHitSlider.value = fillValue;
    }

    private int CalculateDucksToHit(int level)
    {
        if (level < 11)
            return 6;
        else if (level < 13)
            return 7;
        else if (level < 15)
            return 8;
        else
            return 9;
    }
    private float CalculateFillValue()
    {
        if (level <= 1)
            return 6f;
        else if (level <= 10)
            return 6f;
        else if (level <= 12)
            return 7f;
        else if (level <= 14)
            return 8f;
        else
            return 9f;
    }
}
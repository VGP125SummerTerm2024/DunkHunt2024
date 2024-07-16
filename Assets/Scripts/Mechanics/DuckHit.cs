using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Ensure this is included if using TextMeshPro

public class DuckHit : MonoBehaviour
{
    private int level;
    private int ducksToHit;  // Number of ducks that need to be hit to pass the level
    private const int ducksPerLevel = 10;
    private int ducksMissed;
    private int hits;
    private int ducksProcessed; // Counter for processed ducks
    private bool isGameOver;

    public TextMeshProUGUI ducksToHitText;  // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI ducksHitText;    // Reference to the TextMeshProUGUI component

    void Start()
    {
        level = 1;
        ducksToHit = 1;
        hits = 0;
        ducksMissed = 0;
        ducksProcessed = 0; // Initialize counter
        isGameOver = false;
        Debug.Log("Welcome to Duck Hunt!");
        StartLevel();
    }

    void Update()
    {
        if (isGameOver) return;

        // Simulate shooting by pressing space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void StartLevel()
    {
        hits = 0;
        ducksMissed = 0;
        ducksProcessed = 0; // Reset counter
        UpdateUI();
    }

   

    private void Shoot()
    {
        if (ducksProcessed < ducksPerLevel)
        {
            bool hit = Random.Range(0, 2) == 0; // 50% chance to hit a duck
            if (hit)
            {
                hits++;
                Debug.Log("Duck hit!");
            }
            else
            {
                ducksMissed++;
                Debug.Log("Missed!");
            }

            ducksProcessed++;
            if (ducksProcessed >= ducksPerLevel)
            {
                CheckGameState();
            }
        }
    }

    private void CheckGameState()
    {
        if (hits >= ducksToHit)
        {
            Debug.Log($"You hit {hits} ducks! Moving to the next level.");
            NextLevel();
        }
        else
        {
            Debug.Log($"You missed {ducksMissed} ducks and hit {hits} ducks. You need to hit {ducksToHit} ducks to pass the level.");
            if (ducksMissed + hits == ducksPerLevel)
            {
                // If the player has processed all ducks but didn't hit enough
                Debug.Log("Not enough ducks hit. Game Over!");
                isGameOver = true;
                ducksHitText.text = $"Game Over! You hit {hits} ducks.";
            }
        }
    }

    private void NextLevel()
    {
        level++;
        ducksToHit = Mathf.Min(level, 9); // Increase ducks to hit, max is 9
        StartLevel();
    }

    private void UpdateUI()
    {
        ducksToHitText.text = $"Level {level}: Hit {ducksToHit} ducks to advance.";
    }
}

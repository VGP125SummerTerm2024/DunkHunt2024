using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundUI : MonoBehaviour
{
    public Sprite[] digitSprites; // Array to hold digit sprites (0-9)
    public Image tensDigitImage;  // Image component to display the tens digit
    public Image onesDigitImage;  // Image component to display the ones digit

    private int currentRound = 1; // Initial round number

    void Start()
    {
        UpdateRoundUI(); // Initial UI update
    }

    // Method to update the round UI
    private void UpdateRoundUI()
    {
        int tensDigit = currentRound / 10;
        int onesDigit = currentRound % 10;

        if (tensDigit < digitSprites.Length)
        {
            tensDigitImage.sprite = digitSprites[tensDigit];
        }
        else
        {
            Debug.LogWarning("Tens digit exceeds the number of available sprites.");
        }

        if (onesDigit < digitSprites.Length)
        {
            onesDigitImage.sprite = digitSprites[onesDigit];
        }
        else
        {
            Debug.LogWarning("Ones digit exceeds the number of available sprites.");
        }
    }

    // Method to increment the current round and update the UI
    public void IncrementRound()
    {
        currentRound++;
        UpdateRoundUI();
    }

    // Method to set the current round to a specific value and update the UI
    public void SetRound(int round)
    {
        if (round >= 0 && round < 100) // Assuming you don't exceed two-digit rounds
        {
            currentRound = round;
            UpdateRoundUI();
        }
        else
        {
            Debug.LogWarning("Invalid round number.");
        }
    }
}

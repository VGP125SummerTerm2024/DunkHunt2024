using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreButtonsVar1 : MonoBehaviour
{
    // I will try to leave as many comments as possible in this script. I know my coding style may not be the best, but I am still learning :)

    // I plan to make a second version of this script which will follow the old style of scoreing, so all 6 numbers are their own text box - This prevents "1" scaling. We can then decide which path we want to take later.

    // Professor left this comment on the Trello: "ScoreManager.cs with a Getters/Setters. Maybe you make the score manager a singleton?" - Not sure this is totally needed right now.

    // However, I will make a script which follows the above recommendation, then decide which path we want to take later after I get a second opinion.

    public Text score;
    public int scoreValue;
    private int[] scoreArray = new int[6];
    public Button B1;
    public Button B2;
    public Button B3;
    public Text T1;
    public Text T2;
    public Text T3;
    public Text T4;
    public Text T5;
    public Text T6;
    
    void Start()
    {
        T1.text = "0";
        T2.text = "0";
        T3.text = "0";
        T4.text = "0";
        T5.text = "0";
        T6.text = "0";
        score.text = "000000";
    }

    void Update()
    {
        
    }

    public void _BlackDuck() // The method which will be called when the black duck is clicked.
    {
        scoreValue += 500;

        StoreScoreInArray(scoreValue);

        ResetButtonVisual(B1);

        ScoreFormatter();
    }

    public void _BlueDuck() // The method which will be called when the blue duck is clicked.
    {
        scoreValue += 1000;

        StoreScoreInArray(scoreValue);

        ResetButtonVisual(B2);
        
        ScoreFormatter();
    }

    public void _RedDuck() // The method which will be called when the red duck is clicked.
    {
        scoreValue += 1500;

        StoreScoreInArray(scoreValue);

        ResetButtonVisual(B3);

        ScoreFormatter();
    }

    // The method which will reset the button visual. This precents the "Selected" colour from being applied to the button.
    private void ResetButtonVisual(Button button)
    {
        button.interactable = false;
        button.interactable = true;
    }

    public void ScoreFormatter() // The method which will format the score text. Duck hunt uses the old style of scoring, so we need to format it to look like that.
    {
        T1.text = scoreArray[5].ToString();
        T2.text = scoreArray[4].ToString();
        T3.text = scoreArray[3].ToString();
        T4.text = scoreArray[2].ToString();
        T5.text = scoreArray[1].ToString();
        T6.text = scoreArray[0].ToString();
        score.text = scoreValue.ToString();
    }

    public void StoreScoreInArray(int scoreValue) // The method which will store the score in an array.
    {
        string scoreString = scoreValue.ToString("D6"); // Convert to string with 6 digits

        for (int i = 0; i < 6; i++)
        {
            scoreArray[i] = int.Parse(scoreString[i].ToString());
        }
    }
}

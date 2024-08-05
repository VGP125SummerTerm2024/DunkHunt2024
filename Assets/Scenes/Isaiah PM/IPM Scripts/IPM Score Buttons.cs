using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPMScoreButtons : MonoBehaviour
{
    // I will try to leave as many comments as possible in this script. I know my coding style may not be the best, but I am still learning :)

    // I plan to make a second version of this script which will follow the old style of scoreing, so all 6 numbers are their own text box - This prevents "1" scaling. We can then decide which path we want to take later.

    // Professor left this comment on the Trello: "ScoreManager.cs with a Getters/Setters. Maybe you make the score manager a singleton?" - Not sure this is totally needed right now.

    // However, I will make a script which follows the above recommendation, then decide which path we want to take later after I get a second opinion.

    public Text score;
    public int scoreValue;
    public Button B1;
    public Button B2;
    public Button B3;
    
    void Start()
    {
        score.text = "000000"; // Doing this just so that we're not displaying null value.
    }

    void Update()
    {
        
    }

    private void BlackDuck() // The method which will be called when the black duck is clicked.
    {
        scoreValue += 500;

        // Scroll down to see method.
        ResetButtonVisual(B1);

        ScoreFormatter();
    }

    public void BlueDuck() // The method which will be called when the blue duck is clicked.
    {
        scoreValue += 1000;

        ResetButtonVisual(B2);
        
        ScoreFormatter();
    }

    public void RedDuck() // The method which will be called when the red duck is clicked.
    {
        scoreValue += 1500;

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

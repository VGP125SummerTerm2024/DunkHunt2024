using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClayRoundManager : MonoBehaviour
{
    public AmmoManager ammoManager;
    public ClayTargetSpawner targetSpawner;
    public ClayHit_UI HitUI;

    public GameObject startCanvas;
    public GameObject roundIndicator;
    public GameObject gameOverCanvas;
    public GameObject roundScoreCanvas;
    public int round;

    private bool isRoundActive = false;
    private bool isWaitingForRound = false;
    bool spawning = false;
    public int _roundCount;
    public bool gameover = false;
    void Start()
    {
        _roundCount=1;
        StartNewRound();
    }

    void Update()
    {
        
        if (!isRoundActive && !isWaitingForRound && !spawning && !gameover)
        {
           //StartCoroutine(StartRound());
        }
    }

    public void StartNewRound()
    {
        Debug.Log("Starting New Round");
        startCanvas.SetActive(true);
        roundIndicator.SetActive(true);
        roundIndicator.GetComponent<ClayRoundInd>().display();
        isWaitingForRound = true;

        if (gameover)
            return;
        StartCoroutine(WaitForRound());
    }

    IEnumerator WaitForRound()
    {
        yield return new WaitForSeconds(4f);
        isWaitingForRound = false;

        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        isRoundActive = true;
        startCanvas.SetActive(false);
        roundIndicator.SetActive(false);

        yield return new WaitForSeconds(3f);

        StartCoroutine(targetSpawner.SpawnTargets());

       

        // Wait until all targets are hit
        while (targetSpawner.spawnedTargets > 0)
        {
            yield return null;
        }

        isRoundActive = false;
        StartCoroutine(NextRound());
    }

    IEnumerator NextRound()
    {
        targetSpawner.spawnedTargets = 0;

        //roundScoreCanvas.SetActive(true);
        HitUI.CheckGameState();
        round++;
        ++_roundCount;

        // Check if the game should end
        if (_roundCount >= 10)
        {
            gameOver();
        }
        else
        {
            yield return new WaitForSeconds(1f);
            roundScoreCanvas.SetActive(false);
            //StartNewRound();
        }
        
    }

    public void gameOver()
    {
        StartCoroutine(EndGame());
    }
    IEnumerator EndGame()
    {
        gameOverCanvas.SetActive(true);
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("IPM Main Menu");
    }

    public int getRoundNumber()
    {
        return _roundCount;
    }
}

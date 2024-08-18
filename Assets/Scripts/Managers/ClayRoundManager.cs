using System.Collections;
using UnityEngine;

public class ClayRoundManager : MonoBehaviour
{
    public AmmoManager ammoManager;
    public ClayTargetSpawner targetSpawner;
    public DuckHit HitUI;

    public GameObject startCanvas;
    public GameObject roundIndicator;
    public GameObject gameOverCanvas;
    public GameObject roundScoreCanvas;
    public int round;

    private bool isRoundActive = false;
    private bool isWaitingForRound = false;
    bool spawning = false;
    public int _roundCount;
    void Start()
    {
        _roundCount=1;
        StartNewRound();
    }

    void Update()
    {
        
        if (!isRoundActive && !isWaitingForRound && !spawning )
        {
           // StartCoroutine(StartRound());
        }
    }

    void StartNewRound()
    {
        startCanvas.SetActive(true);
        roundIndicator.SetActive(true);
        isWaitingForRound = true;

        StartCoroutine(WaitForRound());
    }

    IEnumerator WaitForRound()
    {
        yield return new WaitForSeconds(3f);
        isWaitingForRound = false;

        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        isRoundActive = true;
        startCanvas.SetActive(false);
        roundIndicator.SetActive(false);

        yield return new WaitForSeconds(1f);

        StartCoroutine(targetSpawner.SpawnTargets());

        // Wait until all targets are spawned
        while (targetSpawner.spawnedTargets < targetSpawner.maxTargets)
        {
            yield return null;
        }

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
        // Wait until all targets are hit
        while (targetSpawner.spawnedTargets > 0)
        {
            yield return null;
        }

        roundScoreCanvas.SetActive(true);
        HitUI.CheckGameState();
        round++;
        _roundCount++;

        // Check if the game should end
        if (_roundCount >= 10)
        {
            StartCoroutine(EndGame());
        }
        else
        {
            yield return new WaitForSeconds(1f);
            roundScoreCanvas.SetActive(false);
            StartNewRound();
        }
    }


    IEnumerator EndGame()
    {
        gameOverCanvas.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false); // End the game
    }

    public int getRoundNumber()
    {
        return _roundCount;
    }
}

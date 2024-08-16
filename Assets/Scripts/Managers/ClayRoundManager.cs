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

    void Start()
    {
        StartNewRound();
    }

    void Update()
    {
        
        if (!isRoundActive && !isWaitingForRound && !spawning)
        {
            StartCoroutine(StartRound());
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
    }

    IEnumerator StartRound()
    {
        isRoundActive = true;

        startCanvas.SetActive(false);
        roundIndicator.SetActive(false);

        yield return new WaitForSeconds(1f);

        StartCoroutine(targetSpawner.SpawnTargets());

        yield return StartCoroutine(NextRound());

        isRoundActive = false;
    }

    IEnumerator NextRound()
    {
        while (targetSpawner.spawnedTargets > 0)
        {
            yield return null; // Wait until all targets are hit
        }

        roundScoreCanvas.SetActive(true);
        HitUI.CheckGameState();
        round++;

        yield return new WaitForSeconds(1f);

        if (round == 99)
        {
            StartCoroutine(EndGame());
        }
        else
        {
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
}

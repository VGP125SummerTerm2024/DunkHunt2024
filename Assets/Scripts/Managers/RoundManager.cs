using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    UnityEvent onRoundStarted = new UnityEvent();
    UnityEvent onSubRoundStarted = new UnityEvent();
    UnityEvent onRoundComplete = new UnityEvent();
    private int _roundCount;
    private int _subRoundCount;
    private int _subRoundMax = 10;
    private bool _timer;
    private float _timeLeft;
    private bool roundPlaying = false;
    public DogAI DogAIRef;

    //UI elements that require updates
    [SerializeField] GameObject roundIndicator;
    RoundUI roundUI;
    [SerializeField] GameObject gameOverImage;

    [SerializeField] SpawnManager spawnManager;

    // ammo and round trackers/triggers
    [SerializeField] AmmoManager ammo;
    bool subRoundOver = false;
    bool lost = false;
    [SerializeField] float subRoundTime = 3f;
    [SerializeField] float RoundTime = 6f;
    public bool firstRound = true;
    bool spawning = false;

    // duck variables
    public int ducks;
    [SerializeField]public Dictionary<string, GameObject> duckObj = new Dictionary<string, GameObject>();
    public float duckSpeedMult;

    

    // Start is called before the first frame update
    private void Start()
    {
        ducks = 0;
        duckSpeedMult = 1;
        _roundCount = 1;
        _subRoundCount = 1;
        roundUI = FindObjectOfType<RoundUI>();
        switch (GameManager.Instance.GameMode)
        {
            case 1: // 1 Duck Mode
                _subRoundMax = 10;
                break;
            case 2: // 2 Duck mode
                _subRoundMax = 5;
                break;
            case 3: // Clay Disk mode
                _subRoundMax = 10;
                break;
        }
    }
    private void Update()
    {
        if (ducks > 0)
            roundPlaying = true;
        else
            roundPlaying = false;

        // if spawning dont check for ducks or ammo round enders
        if (!spawning)
        {
            // if the player cleared the ducks trigger the doghold duck and increment subround
            if (ducks <= 0 && firstRound == false && !subRoundOver && !spawning)
            {
                subRoundOver = true;
                AdvanceRound();
                StartCoroutine(dogDelay("HoldDuck"));
            }
            //else if player ran out of ammo mock player and make ducks fly away, increment round
            else if (ammo.getAmmo() <= 0 && !subRoundOver && ducks > 0)
            {
                subRoundOver = true;
                missedDucks();
                AdvanceRound();
                StartCoroutine(dogDelay("Laugh"));
                
            }
            // tracks if player clicked and shot a bullet
            if (Input.GetMouseButtonDown(0) && roundPlaying)
            {
                ammo.UpdateAmmo();
            }
        }
        if (_timer)
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                StartNextRound();
                _timer = false;
            }
        }
    }

    IEnumerator dogDelay(string type)
    {
        DogAIRef.MoveToDuckPosition(transform.position);
        yield return new WaitForSeconds(1.0f);
        if (type == "Laugh")
            DogAIRef.DogLaugh();
        else if (type == "HoldDuck")
            DogAIRef.DogHoldDuck();
    }
    
    private void AdvanceRound()
    {
        if (lost) return;

        if (_subRoundCount < _subRoundMax)
        {
            // increments sub round and spawns duck after time
            _subRoundCount += 1;
            onSubRoundStarted.Invoke();
            StartCoroutine(waitForSpawnTime(subRoundTime));
        }
        else
        {
            // adjust the variables of resetting the subround count and begining a new round
            _roundCount++;
            _subRoundCount = 1;
            setSpeedMult();
            //dog walk but shorter
            roundIndicator.SetActive(true);
            roundIndicator.GetComponent<RoundIndicator>().display();
            roundUI.IncrementRound();
            StartCoroutine(waitForSpawnTime(RoundTime));
            onRoundComplete.Invoke();
            IPMScoreManager.Instance.RoundMultiplier(_roundCount);
            _timeLeft = 5f;
            _timer = true;
        }
    }

    // goes through ducks and triggers them to fly away
    void missedDucks()
    {
        foreach (var d in duckObj.Values)
        {
            d.GetComponent<DuckScript>().FlyAway();
        }
        duckObj.Clear();
    }

    // Removes duck from the dictionary
    public void onDuckDestroy(GameObject duck)
    {
        ducks--;
        duckObj.Remove(duck.GetComponent<DuckScript>().duckName);
    }

    void StartNextRound()
    {
        onRoundStarted.Invoke();
    }

    public int getRoundNumber()
    {
        return _roundCount;
    }

    public void gameOver()
    {
        StartCoroutine(endGame(6.0f));
    }

    // resets the ammo and begins the new round after a given time
    IEnumerator waitForSpawnTime(float time)
    {
        spawning = true;
        yield return new WaitForSeconds(time);
        ammo.reload();
        spawnManager.DuckSpawner();
        subRoundOver = false;
        spawning = false;
    }

    // gives the player a game over and updates the highscore is applicable, loads back to menu
    IEnumerator endGame(float time)
    {
        lost = true;
        GameManager.Instance.UpdateHighScore(IPMScoreManager.Instance.scoreValue);
        gameOverImage.SetActive(true);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("IPM Main Menu");
    }

    // calculates the speed multiplier of the ducks
    void setSpeedMult()
    {
        if (_roundCount <= 8)
            duckSpeedMult = 1 + (_roundCount / 8);
        else if (_roundCount <= 12)
            duckSpeedMult = 2 + ((_roundCount % 8) / 4);
        else
            duckSpeedMult = 3;
    }
}

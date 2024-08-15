using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    UnityEvent onRoundStarted = new UnityEvent();
    UnityEvent onSubRoundStarted = new UnityEvent();
    UnityEvent onRoundComplete = new UnityEvent();
    private int _roundCount;
    [SerializeField]private int _subRoundCount;
    private int _subRoundMax = 10;
    private bool _timer;
    private float _timeLeft;
    public DogAI DogAIRef;

    [SerializeField] GameObject roundIndicator;
    RoundUI roundUI;

    [SerializeField] SpawnManager spawnManager;

    [SerializeField] float subRoundTime = 3f;
    [SerializeField] float RoundTIme = 6f;
    public bool firstRound = true;
    bool spawning = false;

    public int ducks;
    public Dictionary<string, GameObject> duckObj = new Dictionary<string, GameObject>();

    [SerializeField] AmmoManager ammo;
    bool subRoundOver = false;
    bool roundOver = false;

    // Start is called before the first frame update
    private void Start()
    {
        ducks = 0;
        DogAIRef = new DogAI();
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
        if (!spawning)
        {
            if (ducks <= 0 && firstRound == false && !subRoundOver && !spawning)
            {
                subRoundOver = true;
                AdvanceRound();
                //DogAIRef.DogHoldDuck();
            }
            else if (ammo.getAmmo() <= 0 && !subRoundOver && ducks > 0)
            {
                subRoundOver = true;
                missedDucks();
                AdvanceRound();
                //DogAIRef.DogLaugh();
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
    
    private void AdvanceRound()
    {
        if (_subRoundCount < _subRoundMax)
        {
            _subRoundCount += 1;
            onSubRoundStarted.Invoke();
            StartCoroutine(waitForSpawnTime(subRoundTime));
        }
        else
        {
            _roundCount++;
            _subRoundCount = 1;
            //dog walk but shorter
            roundIndicator.SetActive(true);
            roundIndicator.GetComponent<RoundIndicator>().display();
            roundUI.IncrementRound();
            StartCoroutine(waitForSpawnTime(RoundTIme));
            onRoundComplete.Invoke();
            _timeLeft = 5f;
            _timer = true;
        }
        // Spawn Ducks | Can't use instance right now.
    }

    void missedDucks()
    {
        foreach (var d in duckObj.Values)
        {
            d.GetComponent<DuckScript>().FlyAway();
        }
        duckObj.Clear();
    }

    public void onDuckDestroy(GameObject duck)
    {
        ducks--;
        duckObj.Remove(duck.name);
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

    }

    IEnumerator waitForSpawnTime(float time)
    {
        spawning = true;
        yield return new WaitForSeconds(time);
        ammo.reload();
        spawnManager.DuckSpawner();
        subRoundOver = false;
        spawning = false;
    }
}

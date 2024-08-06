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
    private int _subRoundCount;
    private int _subRoundMax;
    private bool _timer;
    private float _timeLeft;
    public DogAI DogAIRef;
    public int ducks;

    // Start is called before the first frame update
    private void Start()
    {
        DogAIRef = new DogAI();
        _roundCount = 1;
        _subRoundCount = 1;
        switch (GameManager.Instance.GameMode)
        {
            case 1: // 1 Duck Mode
                _subRoundMax = 10;
                break;
            case 2: // 2 Duck mode
                _subRoundCount = 5;
                break;
            case 3: // Clay Disk mode
                _subRoundMax = 10;
                break;
        }
    }
    private void Update()
    {
        if (_timer)
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft < 0)
            {
                StartNextRound();
                _timer = false;
            }
        }
    }
    // Expecting outside scripts to call on advance sub round
    public void AdvanceRound()
    {
        if (_subRoundCount < _subRoundMax)
        {
            _subRoundCount += 1;
            onSubRoundStarted.Invoke();
        }
        else
        {
            onRoundComplete.Invoke();
            _timeLeft = 5f;
            _timer = true;
        }
        // Spawn Ducks | Can't use instance right now.
    }


    void StartNextRound()
    {
        onRoundStarted.Invoke();
    }

    public int getRoundNumber()
    {
        return _roundCount;
    }
}

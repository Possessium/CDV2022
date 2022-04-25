using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsGameStarted { get; private set; } = false;
    public float CurrentTime { get; private set; }

    public event Action OnGameStart = null;
    public event Action<int, int> OnScoreUpdate = null;
    public event Action<float> OnTimeUpdate = null;
    public event Action<int, int> OnGameEnded = null;

    private int scoreP1 = 0;
    private int scoreP2 = 0;

    private void Awake()
    {
        CurrentTime = 0;
        Instance = this;
    }


    private void Update()
    {
        UpdateTimer();
    }

    /// <summary>
    /// Initialize the game
    /// </summary>
    public void StartGame()
    {
        IsGameStarted = true;
        OnGameStart?.Invoke();
    }

    /// <summary>
    /// Increase time and call event on game time end
    /// </summary>
    private void UpdateTimer()
    {
        if (!IsGameStarted)
            return;

        CurrentTime += Time.deltaTime;
        OnTimeUpdate?.Invoke(CurrentTime);

        if (CurrentTime > 120)
        {
            OnGameEnded?.Invoke(scoreP1, scoreP2);
            Destroy(this);
        }
    }

    /// <summary>
    /// Add the score to the player count
    /// </summary>
    /// <param name="_amount">int : Amount to add to the player</param>
    /// <param name="_isp1">bool : Identifies which player to add the score</param>
    public void AddScore(int _amount, bool _isp1)
    {
        if (_isp1)
            scoreP1 += _amount;
        else
            scoreP2 += _amount;

        OnScoreUpdate?.Invoke(scoreP1, scoreP2);
    }

    private void OnDestroy()
    {
        // Safety
        OnScoreUpdate = null;
        OnTimeUpdate = null;
        OnGameEnded = null;
    }
}

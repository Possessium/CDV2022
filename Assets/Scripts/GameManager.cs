using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action<int, int> OnScoreUpdate = null;
    public event Action<float> OnTimeUpdate = null;
    public event Action<int, int> OnGameEnded = null;

    private int scoreP1 = 0;
    private int scoreP2 = 0;
    [SerializeField] private float remainingTime = 120;

    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        UpdateTimer();
    }

    /// <summary>
    /// Decrease time and call event on game time end
    /// </summary>
    private void UpdateTimer()
    {
        remainingTime -= Time.deltaTime;
        OnTimeUpdate?.Invoke(remainingTime);

        if (remainingTime <= 0)
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

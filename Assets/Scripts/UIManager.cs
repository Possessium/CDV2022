using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreP1;
    [SerializeField] private TMP_Text scoreP2;
    
    [SerializeField] private TMP_Text timer;
    
    [SerializeField] private TMP_Text highScoreSolo;
    [SerializeField] private TMP_Text highScoreDuo;

    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject endUI;

    private void Start()
    {
        GameManager.Instance.OnScoreUpdate += UpdateScores;
        GameManager.Instance.OnTimeUpdate += UpdateTime;
        GameManager.Instance.OnGameEnded += ChangeUIToEnd;

        EndGameManager.Instance.OnDuoHighscore += DuoHighScore;
        EndGameManager.Instance.OnSoloHighscore += SoloHighScore;
    }

    /// <summary>
    /// Update the UI of the player scores
    /// </summary>
    /// <param name="_player1">int : new value of the player1 score</param>
    /// <param name="_player2">int : new value of the player2 score</param>
    public void UpdateScores(int _player1, int _player2)
    {
        scoreP1.text = _player1.ToString();
        scoreP2.text = _player2.ToString();
    }

    /// <summary>
    /// Update the UI of the Timer
    /// </summary>
    /// <param name="_remainingTime">float : new value of the Timer</param>
    public void UpdateTime(float _remainingTime)
    {
        TimeSpan _ts = TimeSpan.FromSeconds(_remainingTime);

        timer.text = _ts.ToString(@"mm\:ss");
    }

    /// <summary>
    /// Update the solo highscore
    /// </summary>
    /// <param name="_amount">New highscore</param>
    /// <param name="_isPlayer1">bool : Which player has done the Highscore</param>
    public void SoloHighScore(int _amount, bool _isPlayer1)
    {
        highScoreSolo.text = $"New Solo HighScore by {(_isPlayer1 ? "Player 1" : "Player 2")} : {_amount}";
    }

    /// <summary>
    /// Update the Duo Highscore
    /// </summary>
    /// <param name="_amount">New highscore</param>
    public void DuoHighScore(int _amount)
    {
        highScoreDuo.gameObject.SetActive(true);
        highScoreDuo.text = $"New duo HighScore : {_amount}";
    }

    /// <summary>
    /// Disable the GameUI and Enable the EndUI
    /// </summary>
    /// <param name="_useless">int : here only to be able to call from event</param>
    /// <param name="_useless2">int : here only to be able to call from event</param>
    public void ChangeUIToEnd(int _useless, int _useless2)
    {
        gameUI.SetActive(false);
        endUI.SetActive(true);
    }
}

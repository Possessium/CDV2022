using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndGameManager : MonoBehaviour
{
    public static EndGameManager Instance;

    private const string SOLOHIGHSCORETAG = "SoloHighScore";
    private const string DUOHIGHSCORETAG = "DuoHighScore";

    private int player1Score;
    private int player2Score;

    public event Action<int, bool> OnSoloHighscore = null;
    public event Action<int> OnDuoHighscore = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGameEnded += RegisterScores;

        OnSoloHighscore += RegisterHighScoreSolo;
        OnDuoHighscore += RegisterHighScoreDuo;
    }

    /// <summary>
    /// Saves the score of both players and start the logic of highscores
    /// </summary>
    /// <param name="_player1"></param>
    /// <param name="_player2"></param>
    public void RegisterScores(int _player1, int _player2)
    {
        player1Score = _player1;
        player2Score = _player2;

        CheckHighScores();
    }

    /// <summary>
    /// Retrieves the highscores and compares it to the current scores
    /// </summary>
    private void CheckHighScores()
    {
        // Get the highscores from PlayerPrefs
        int _soloHighscore = PlayerPrefs.GetInt(SOLOHIGHSCORETAG);
        int _duoHighscore = PlayerPrefs.GetInt(DUOHIGHSCORETAG);

        // Look for a new highscore in solo
        if (player1Score > _soloHighscore)
        {
            if (player2Score > _soloHighscore)
            {
                OnSoloHighscore?.Invoke(player2Score, false);
            }
            else
                OnSoloHighscore?.Invoke(player1Score, true);
        }

        else if (player2Score > _soloHighscore)
        {
            OnSoloHighscore?.Invoke(player2Score, false);
        }

        // Look for a new highscore in duo
        if (player1Score + player2Score > _duoHighscore)
        {
            OnDuoHighscore?.Invoke(player1Score + player2Score);
        }
    }

    public void RegisterHighScoreSolo(int _amount, bool _useless) => PlayerPrefs.SetInt(SOLOHIGHSCORETAG, _amount);
    public void RegisterHighScoreDuo(int _amount) => PlayerPrefs.SetInt(DUOHIGHSCORETAG, _amount);

    private void OnDestroy()
    {
        // Safety
        OnSoloHighscore = null;
        OnDuoHighscore = null;
    }
}

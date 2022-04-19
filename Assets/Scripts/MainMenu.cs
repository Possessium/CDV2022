using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image player1Button;
    [SerializeField] private Sprite player1readySprite;
    [SerializeField] private Image player2Button;
    [SerializeField] private Sprite player2readySprite;

    private bool isPlayer1Ready = false;
    private bool isPlayer2Ready = false;

    private const string GAME_SCENE_NAME = "Scene_Hogu";


    public void RegisterPlayer1(InputAction.CallbackContext _ctx)
    {
        if (!_ctx.started || isPlayer1Ready)
            return;

        isPlayer1Ready = true;
        player1Button.sprite = player1readySprite;

        if (isPlayer2Ready)
            StartGame();
    }

    public void RegisterPlayer2(InputAction.CallbackContext _ctx)
    {
        if (!_ctx.started || isPlayer2Ready)
            return;

        isPlayer2Ready = true;
        player2Button.sprite = player2readySprite;

        if (isPlayer1Ready)
            StartGame();
    }

    private void StartGame()
    {
        SceneManager.LoadScene(GAME_SCENE_NAME);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Image player1Button;
    [SerializeField] private Sprite player1readySprite;
    [SerializeField] private Image player2Button;
    [SerializeField] private Sprite player2readySprite;

    private bool isPlayer1Ready = false;
    private bool isPlayer2Ready = false;


    private void Start()
    {
        AnimationCaller.Instance.OnMenuAnimationEnded += StartGame;
    }

    /// <summary>
    /// Register the input of Player 1
    /// </summary>
    /// <param name="_ctx">InputAction.CallbackContext : Input system context</param>
    public void RegisterPlayer1(InputAction.CallbackContext _ctx)
    {
        if (!_ctx.started || isPlayer1Ready)
            return;

        isPlayer1Ready = true;
        player1Button.sprite = player1readySprite;

        // If the other player is ready start the countdown
        if (isPlayer2Ready)
            StartAnim();
    }

    /// <summary>
    /// Register the input of Player 2
    /// </summary>
    /// <param name="_ctx">InputAction.CallbackContext : Input system context</param>
    public void RegisterPlayer2(InputAction.CallbackContext _ctx)
    {
        if (!_ctx.started || isPlayer2Ready)
            return;

        isPlayer2Ready = true;
        player2Button.sprite = player2readySprite;

        // If the other player is ready start the countdown
        if (isPlayer1Ready)
            StartAnim();
    }

    /// <summary>
    /// Start the animation of the countdown of the start of the game
    /// </summary>
    private void StartAnim() => animator.SetTrigger("Start");

    /// <summary>
    /// Calls the start of the game on the GameManager
    /// Destroy the MainMenu to optimize
    /// </summary>
    public void StartGame()
    {
        GameManager.Instance.StartGame();
        Destroy(transform.parent.gameObject);
    }
}

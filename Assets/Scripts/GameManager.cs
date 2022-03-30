using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private int scoreP1 = 0;
    private int scoreP2 = 0;

    private void Start()
    {
        instance = this;
    }

    public void AddScore(int _amount, bool _isp1)
    {
        if (_isp1)
            scoreP1 += _amount;
        else
            scoreP2 += _amount;
    }

}

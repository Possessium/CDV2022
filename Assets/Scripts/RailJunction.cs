using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailJunction : MonoBehaviour
{
    [SerializeField] private bool canGoTop;
    [SerializeField] private bool canGoBottom;
    [SerializeField] private bool canGoLeft;
    [SerializeField] private bool canGoRight;


    /// <summary>
    /// Returns true if the player can go towards the given direction
    /// </summary>
    /// <param name="_dir">Direction to search</param>
    /// <returns></returns>
    public bool CanGoTowards(Directions _dir)
    {
        switch (_dir)
        {
            case Directions.Top:
                return canGoTop;
            case Directions.Bottom:
                return canGoBottom;
            case Directions.Left:
                return canGoLeft;
            case Directions.Right:
                return canGoRight;
        }
        return false;
    }
}

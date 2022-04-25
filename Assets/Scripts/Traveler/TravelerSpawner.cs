using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelerSpawner : MonoBehaviour
{
    public bool IsRightSide;

    private void OnDrawGizmos()
    {
        Gizmos.color = IsRightSide ? Color.magenta : Color.cyan;
        Gizmos.DrawSphere(transform.position, .4f);
    }
}

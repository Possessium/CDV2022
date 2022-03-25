using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [SerializeField] private Vector3 zoneBounds;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255, 165, 0, .25f);
        Gizmos.DrawCube(transform.position, zoneBounds * 2);
    }

    /// <summary>
    /// Return a random Vector3 point inside the zone
    /// </summary>
    /// <returns> Vector3 : point inside the zone</returns>
    public Vector3 GetRandomPoint()
    {
        return transform.position + new Vector3(Random.Range(-zoneBounds.x, zoneBounds.x), 0, Random.Range(-zoneBounds.z, zoneBounds.z));
    }
} 

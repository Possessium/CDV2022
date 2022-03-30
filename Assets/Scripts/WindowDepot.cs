using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowDepot : MonoBehaviour
{
    [SerializeField] private Collider depotCollider;

    private void Start()
    {
        TryGetComponent(out depotCollider);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(depotCollider)
            Gizmos.DrawWireCube(depotCollider.bounds.center, depotCollider.bounds.size);
    }

    public void EnableCollider() => depotCollider.enabled = true;
    public void DisableCollider() => depotCollider.enabled = false;
}

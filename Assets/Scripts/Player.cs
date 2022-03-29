using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

public class Player : MonoBehaviour
{
    [SerializeField] private float radius = 5;
    [SerializeField] private float speed = 1;
    [SerializeField] private LayerMask travelerLayer = 0;
    [SerializeField] private Bounds playerLimits;

    private List<Traveler> grabbedTravelers = new List<Traveler>();

    [SerializeField] private Vector2 inputMovement;

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Handles.DrawSolidDisc(new Vector3(transform.position.x, 0, transform.position.z), Vector3.up, radius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerLimits.center, playerLimits.extents);
    }

    private void Update()
    {
        Vector3 _nextPosition = Vector3.MoveTowards(transform.position, transform.position + new Vector3(inputMovement.x, 0, inputMovement.y), Time.deltaTime * speed);

        if (playerLimits.Contains(_nextPosition))
            transform.position = _nextPosition;
    }

    #region Inputs
    public void Interact(InputAction.CallbackContext _ctx)
    {
        if (!_ctx.started)
            return;

        if (grabbedTravelers.Count != 0)
            ReleaseTravelers();
        else
            GrabTravelers();
    }

    public void MovePlayer(InputAction.CallbackContext _ctx)
    {
        inputMovement = _ctx.ReadValue<Vector2>();
    }

    #endregion

    private void ReleaseTravelers()
    {
        // Check la porte
            // True : lacher les traveler dans la porte (bloquer le système d'agent pour pas qu'ils tp comme des connards au bord de leur zone)
        // False : ce qui a en dessous

        foreach (Traveler _traveler in grabbedTravelers)
        {
            _traveler.transform.position = transform.position + Vector3.up;
            _traveler.gameObject.SetActive(true);
        }
        grabbedTravelers.Clear();
    }
    private void GrabTravelers()
    {
        RaycastHit[] _hits = Physics.SphereCastAll(transform.position, radius, Vector3.up, 4, travelerLayer);
        foreach (RaycastHit _hit in _hits)
        {
            if (_hit.transform.GetComponent<Traveler>())
            {
                grabbedTravelers.Add(_hit.transform.GetComponent<Traveler>());
                _hit.transform.gameObject.SetActive(false);
            }
        }
    }
}

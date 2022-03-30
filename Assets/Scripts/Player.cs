using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using System.Linq;

public class Player : MonoBehaviour
{
    [SerializeField, Tooltip("Size of the grab")] private float radius = 5;
    [SerializeField, Tooltip("Speed of the hand")] private float speed = 1;
    [SerializeField, Tooltip("Layer of the Travelers")] private LayerMask travelerLayer = 0;
    [SerializeField, Tooltip("Layer of the Window")] private LayerMask windowLayer = 0;
    [SerializeField, Tooltip("Limits of the player movements")] private Bounds playerLimits;
    [SerializeField] private bool isPlayer1;

    private List<Traveler> grabbedTravelers = new List<Traveler>();

    private Vector2 inputMovement;

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Handles.DrawSolidDisc(new Vector3(transform.position.x, 0, transform.position.z), Vector3.up, radius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerLimits.center, playerLimits.extents);
    }

    private void Update()
    {
        MovePlayer();
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

    /// <summary>
    /// Move the players based on the input and blocks him inside its bounds
    /// </summary>
    private void MovePlayer()
    {
        Vector3 _nextPosition = Vector3.MoveTowards(transform.position, transform.position + new Vector3(inputMovement.x, 0, inputMovement.y), Time.deltaTime * speed);

        // Limits the X position of the player in the bounds
        if (_nextPosition.x < (playerLimits.min.x / 2) + (playerLimits.center.x / 2) || _nextPosition.x > (playerLimits.max.x / 2) + (playerLimits.center.x / 2))
            _nextPosition.x = transform.position.x;

        // Limits the Y position of the player in the bounds
        if (_nextPosition.z < (playerLimits.min.z / 2) + (playerLimits.center.z / 2) || _nextPosition.z > (playerLimits.max.z / 2) + (playerLimits.center.z / 2))
            _nextPosition.z = transform.position.z;

        transform.position = _nextPosition;
    }

    private void ReleaseTravelers()
    {
        // Search for a window in the release area
        RaycastHit[] _hits = Physics.SphereCastAll(transform.position, radius, Vector3.up, 4, windowLayer);
        if (_hits.Any( _h => _h.transform.GetComponent<WindowDepot>()))
        {
            // Get the Window if found any and gives it to each grabbed Travelers while releasing them
            WindowDepot _d = _hits.Select(_h => _h.transform.GetComponent<WindowDepot>()).First();

            // Tells the manager to remove the number of grabbed Travelers to update the current number in the move area
            TravelerManager.instance.RemoveTravelers(grabbedTravelers.Count - 1, isPlayer1);

            int _scoreToAdd = 0;

            foreach (Traveler _traveler in grabbedTravelers)
            {
                _scoreToAdd += _traveler.ScoreValue;
                _traveler.transform.position = transform.position + Vector3.up;
                _traveler.gameObject.SetActive(true);
                _traveler.SetWindow(_d);
            }

            GameManager.instance.AddScore(_scoreToAdd, isPlayer1);
        }

        else
        {
            // Release each traveler at the hand position
            foreach (Traveler _traveler in grabbedTravelers)
            {
                _traveler.transform.position = transform.position + Vector3.up;
                _traveler.gameObject.SetActive(true);
            }
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

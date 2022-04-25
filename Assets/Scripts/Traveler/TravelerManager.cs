using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TravelerManager : MonoBehaviour
{
    public static TravelerManager instance;

    [SerializeField, Tooltip("Prefabs of the Travelers to spawn")] private Traveler[] travelersPrefab;
    [SerializeField, Tooltip("Number of travelers to spawn at the start of the game")] private int startTravelers = 15;

    [SerializeField, Tooltip("TravelerZone of the player 1")] private TravelerZone p1SpawnZone;
    [SerializeField, Tooltip("TravelerZone of the player 2")] private TravelerZone p2SpawnZone;

    [SerializeField] private AnimationCurve populationCurve;

    private List<TravelerSpawner> allSpawnerRight = new List<TravelerSpawner>();
    private List<TravelerSpawner> allSpawnerLeft = new List<TravelerSpawner>();

    private List<Traveler> travelerPool = new List<Traveler>();

    private int p1Travelers = 0;
    private int p2Travelers = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGameStart += InitializeTravelers;
        GameManager.Instance.OnTimeUpdate += UpdateTime;
    }

    /// <summary>
    /// Update the Traveler count based on the value of the curve on the given time
    /// </summary>
    /// <param name="_time">float : time given to evaluate the curve at</param>
    public void UpdateTime(float _time)
    {
        // Get the target population on the populationCurve at the _time
        int _targetPopulation = (int)populationCurve.Evaluate(_time);
        
        // If there is a gap between the P1Travelers and the target
        if (p1Travelers < _targetPopulation)
        {
            // Get the amount missing
            int _missing = _targetPopulation - p1Travelers;

            // Spawn or pull from the pool the required amount of Traveler missing
            Traveler _tempTraveler;
            TravelerSpawner _tempSpawner;
            for (int i = 0; i < _missing; i++)
            {
                _tempSpawner = allSpawnerRight[Random.Range(0, allSpawnerRight.Count - 1)];

                if (travelerPool.Count > 0)
                {
                    _tempTraveler = travelerPool[0];
                    travelerPool.Remove(_tempTraveler);
                    _tempTraveler.transform.position = _tempSpawner.transform.position;
                }
                else
                    _tempTraveler = Instantiate(travelersPrefab[Random.Range(0, travelersPrefab.Length)], _tempSpawner.transform.position, Quaternion.identity);

                _tempTraveler.InitializeAgent(p1SpawnZone, true);
                p1Travelers++;
            }
        }

        // If there is a gap between the P2Travelers and the target
        if (p2Travelers < _targetPopulation)
        {
            // Get the amount missing
            int _missing = _targetPopulation - p2Travelers;

            // Spawn or pull from the pool the required amount of Traveler missing
            Traveler _tempTraveler;
            TravelerSpawner _tempSpawner;
            for (int i = 0; i < _missing; i++)
            {
                _tempSpawner = allSpawnerLeft[Random.Range(0, allSpawnerLeft.Count - 1)];

                if (travelerPool.Count > 0)
                {
                    _tempTraveler = travelerPool[0];
                    travelerPool.Remove(_tempTraveler);
                    _tempTraveler.transform.position = _tempSpawner.transform.position;
                }
                else
                    _tempTraveler = Instantiate(travelersPrefab[Random.Range(0, travelersPrefab.Length)], _tempSpawner.transform.position, Quaternion.identity);

                _tempTraveler.InitializeAgent(p2SpawnZone, false);
                p2Travelers++;
            }
        }
    }

    /// <summary>
    /// Spawn the firsts Travelers
    /// </summary>
    public void InitializeTravelers()
    {
        // Get all the spawner in the world
        TravelerSpawner[] _allSpawner = (TravelerSpawner[])FindObjectsOfType(typeof(TravelerSpawner));

        // Populate the correct List based on the side of the spawner
        for (int i = 0; i < _allSpawner.Length; i++)
        {
            if (_allSpawner[i].IsRightSide)
                allSpawnerRight.Add(_allSpawner[i]);
            else
                allSpawnerLeft.Add(_allSpawner[i]);
        }

        // Spawns the first startTravelers for each sides
        Traveler _tempTraveler;
        TravelerSpawner _tempSpawner;
        for (int i = 0; i < startTravelers; i++)
        {
            _tempSpawner = allSpawnerRight[Random.Range(0, allSpawnerRight.Count - 1)];
            _tempTraveler = Instantiate(travelersPrefab[Random.Range(0, travelersPrefab.Length)], _tempSpawner.transform.position, Quaternion.identity);
            _tempTraveler.InitializeAgent(p1SpawnZone, true);
            p1Travelers++;

            _tempSpawner = allSpawnerLeft[Random.Range(0, allSpawnerLeft.Count - 1)];
            _tempTraveler = Instantiate(travelersPrefab[Random.Range(0, travelersPrefab.Length)], _tempSpawner.transform.position, Quaternion.identity);
            _tempTraveler.InitializeAgent(p2SpawnZone, false);
            p2Travelers++;
        }
    }

    /// <summary>
    /// Adds the Traveler to the pool of Traveler
    /// </summary>
    /// <param name="_t">Traveler : Traveler to add</param>
    public void SetTravelerToPool(Traveler _t)
    {
        travelerPool.Add(_t);
        _t.transform.position = Vector3.down * 10;
        _t.StopMoving();
    }

    /// <summary>
    /// Remove the amount of travelers of the correct player
    /// </summary>
    /// <param name="_amount">int : amount to remove</param>
    /// <param name="_isP1">bool : Is player 1</param>
    public void RemoveTravelers(int _amount, bool _isP1)
    {
        if(_isP1)
            p1Travelers -= _amount;

        else
            p2Travelers -= _amount;
    }
}

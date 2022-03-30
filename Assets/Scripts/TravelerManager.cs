using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TravelerManager : MonoBehaviour
{
    public static TravelerManager instance;

    [SerializeField, Tooltip("Prefab of the Traveler to spawn")] private Traveler travelerPrefab;
    [SerializeField, Tooltip("Number of travelers to spawn at the start of the game")] private int startTravelers = 15;

    [SerializeField, Tooltip("TravelerZone of the player 1")] private TravelerZone p1SpawnZone;
    [SerializeField, Tooltip("TravelerZone of the player 2")] private TravelerZone p2SpawnZone;

    [SerializeField] private AnimationCurve populationCurve;

    private List<TravelerSpawner> allSpawnerRight = new List<TravelerSpawner>();
    private List<TravelerSpawner> allSpawnerLeft = new List<TravelerSpawner>();

    private int p1Travelers = 0;
    private int p2Travelers = 0;

    private float gameTime = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Get all the spawner in the world
        TravelerSpawner[] _allSpawner = (TravelerSpawner[])FindObjectsOfType(typeof(TravelerSpawner));

        // Populate the correct List based on the side of the spawner
        for (int i = 0; i < _allSpawner.Length; i++)
        {
            if(_allSpawner[i].IsRightSide)
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
            _tempTraveler = Instantiate(travelerPrefab, _tempSpawner.transform.position, Quaternion.identity);
            _tempTraveler.InitializeAgent(p1SpawnZone, true);
            p1Travelers++;

            _tempSpawner = allSpawnerLeft[Random.Range(0, allSpawnerLeft.Count - 1)];
            _tempTraveler = Instantiate(travelerPrefab, _tempSpawner.transform.position, Quaternion.identity);
            _tempTraveler.InitializeAgent(p2SpawnZone, false);
            p2Travelers++;
        }
    }

    private void Update()
    {
        gameTime += Time.deltaTime;
        
        int _targetPopulation = (int)populationCurve.Evaluate(gameTime);

        if(p1Travelers < _targetPopulation)
        {
            int _missing = _targetPopulation - p1Travelers;

            Traveler _tempTraveler;
            TravelerSpawner _tempSpawner;
            for (int i = 0; i < _missing; i++)
            {
                _tempSpawner = allSpawnerRight[Random.Range(0, allSpawnerRight.Count - 1)];
                _tempTraveler = Instantiate(travelerPrefab, _tempSpawner.transform.position, Quaternion.identity);
                _tempTraveler.InitializeAgent(p1SpawnZone, true);
                p1Travelers++;
            }
        }


        if (p2Travelers < _targetPopulation)
        {
            int _missing = _targetPopulation - p2Travelers;

            Traveler _tempTraveler;
            TravelerSpawner _tempSpawner;
            for (int i = 0; i < _missing; i++)
            {
                _tempSpawner = allSpawnerLeft[Random.Range(0, allSpawnerRight.Count - 1)];
                _tempTraveler = Instantiate(travelerPrefab, _tempSpawner.transform.position, Quaternion.identity);
                _tempTraveler.InitializeAgent(p2SpawnZone, false);
                p2Travelers++;
            }
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TravelerManager : MonoBehaviour
{
    [SerializeField] private Traveler travelerPrefab;
    [SerializeField] private int startTravelers = 15;

    [SerializeField] private TravelerZone p1SpawnZone;
    [SerializeField] private TravelerZone p2SpawnZone;

    private List<TravelerSpawner> allSpawnerRight = new List<TravelerSpawner>();
    private List<TravelerSpawner> allSpawnerLeft = new List<TravelerSpawner>();

    private int p1Travelers = 0;
    private int p2Travelers = 0;


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


}

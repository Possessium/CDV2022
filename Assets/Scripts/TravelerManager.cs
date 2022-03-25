using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelerManager : MonoBehaviour
{
    [SerializeField] private Traveler travelerPrefab;
    [SerializeField] private int startTravelers = 15;

    [SerializeField] private SpawnZone p1SpawnZone;
    [SerializeField] private SpawnZone p2SpawnZone;

    private int p1Travelers = 0;
    private int p2Travelers = 0;


    private void Start()
    {
        // Spawns the first startTravelers for each sides
        Traveler _tempTraveler;
        for (int i = 0; i < startTravelers; i++)
        {
            _tempTraveler = Instantiate(travelerPrefab, p1SpawnZone.GetRandomPoint(), Quaternion.identity);
            _tempTraveler.InitializeAgent(p1SpawnZone);
            p1Travelers++;

            _tempTraveler = Instantiate(travelerPrefab, p2SpawnZone.GetRandomPoint(), Quaternion.identity);
            _tempTraveler.InitializeAgent(p2SpawnZone);
            p2Travelers++;
        }
    }


}

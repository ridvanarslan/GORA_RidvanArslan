using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AstreoidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] asteroidObjects;
    [SerializeField] private int amountAsteroidsToSpawn = 10;
    [SerializeField] private float minRandomSpawn = -500;
    [SerializeField] private float maxRandomSpawn = 500;

    private void Start()
    {
        SpawnAsteroid();
    }

    private void SpawnAsteroid()
    {
        for (int i = 0; i < amountAsteroidsToSpawn; i++)
        {
            float randomX = Random.Range(minRandomSpawn, maxRandomSpawn);
            float randomY = Random.Range(minRandomSpawn, maxRandomSpawn);
            float randomZ = Random.Range(minRandomSpawn, maxRandomSpawn);
            Vector3 randomSpawnPoint = new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z + randomZ);

            var tempObj = Instantiate(asteroidObjects[Random.Range(0, asteroidObjects.Length)], randomSpawnPoint, Quaternion.identity, this.transform);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,new Vector3(maxRandomSpawn * 2, maxRandomSpawn * 2, maxRandomSpawn * 2));
    }
}
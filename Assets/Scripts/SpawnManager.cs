using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Vector3[] spawnPositions = {new Vector3(14, -3, -2), new Vector3(14, -1, -2), new Vector3(14, 1, -2), new Vector3(14, 3, -2), new Vector3(14, 5, -2)};

    public float spawnRate;

    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnRate, spawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPositions[Random.Range(0, 5)], enemyPrefab.transform.rotation);
    }
}

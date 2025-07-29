using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Vector3[] spawnPositions = {new Vector3(7, 2.05f, -2f), new Vector3(7, 0.75f, -3f), new Vector3(7, -0.55f, -4f), new Vector3(17, -1.85f, -5f), new Vector3(7, -3.15f, -6f) };

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

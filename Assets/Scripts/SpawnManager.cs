using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Vector3[] spawnPositions = {new Vector3(7, 2.75f, -2f), new Vector3(7, 1.75f, -3f), new Vector3(7, 0.75f, -4f), new Vector3(17, -0.25f, -5f), new Vector3(7, -1.25f, -6f) };

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
        int spawnPosIndex = Random.Range(0, 5);
        var enemy = Instantiate(enemyPrefab, spawnPositions[spawnPosIndex], enemyPrefab.transform.rotation);
        enemy.GetComponent<Enemy>().SetLine(spawnPosIndex);
    }
}

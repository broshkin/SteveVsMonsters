using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpawnManager : MonoBehaviour
{
    private Dictionary<float, int[]> spawns;
    private Vector3[] spawnPositions = {new Vector3(7, 2.05f, -2f), new Vector3(7, 0.75f, -3f), new Vector3(7, -0.55f, -4f), new Vector3(17, -1.85f, -5f), new Vector3(7, -3.15f, -6f) };

    public float spawnRate;

    public GameObject[] enemiesPrefab;
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
        var a = Random.Range(0, enemiesPrefab.Length);
        Instantiate(enemiesPrefab[a], spawnPositions[Random.Range(0, 5)], enemiesPrefab[a].transform.rotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public static bool win = false;
    public static bool lose = false;
    public Line[] lines;
    public SpawnManager spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(lose);
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && spawnManager.isAllEnemies)
        {
            win = true;
        }
    }
}

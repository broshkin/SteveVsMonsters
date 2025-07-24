using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [Header("Параметры героя")]
    public int cost;
    public float spawnDelay;
    public float health;

    [Header("Префабы героя и снаряда")]
    public GameObject prefabBody;
    public GameObject prefabBullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [Header("Параметры героя")]
    public int cost;
    public float spawnDelay;
    public float hp;

    [Header("Префабы героя и снаряда")]
    public GameObject prefabBody;
    public GameObject prefabBullet;

    public Line line;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void GetDamage(float num)
    {
        hp -= num;
    }

    public float GetHp()
    {
        return hp;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && other.TryGetComponent<ZombieBoss>(out ZombieBoss comp1))
        {
            GetDamage(150);
        }
    }
}

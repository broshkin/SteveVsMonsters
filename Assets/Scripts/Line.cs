using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public bool is_free = true;
    [SerializeField] private int enemy_count = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy_count == 0)
        {
            is_free = true;
        }
        else
        {
            is_free = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemy_count++;
            other.gameObject.GetComponent<Enemy>().line = this;
        }
        if (other.gameObject.GetComponent<Checker>() != null)
        {
            other.gameObject.GetComponent<Checker>().line = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemy_count--;
        }
        if (other.gameObject.GetComponent<Checker>() != null)
        {
            other.gameObject.GetComponent<Checker>().line = null;
        }
    }
    public bool IsFree()
    {
        return is_free;
    }

    public void MinusEnemy()
    {
        enemy_count--;
    }
}

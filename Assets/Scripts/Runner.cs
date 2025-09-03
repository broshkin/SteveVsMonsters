using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Hero
{

    [SerializeField] private float speed;
    void Start()
    {
        transform.position = new Vector3(-7.75f, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);
        if (transform.position.x > 20)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (other.GetComponent<Enemy>().hp > 0)
            {
                other.gameObject.GetComponent<Enemy>().hp -= 1000;
            }
        }
    }
}

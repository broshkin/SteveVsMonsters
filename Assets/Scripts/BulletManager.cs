using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private float bulletSpeed;
    private float damage;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime);
        if (transform.position.x > 13)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float num)
    {
        bulletSpeed = num;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().GetDamage(damage);
            Destroy(gameObject);
        }
    }

    public void SetDamage(float num) 
    {
        damage = num; 
    }
}

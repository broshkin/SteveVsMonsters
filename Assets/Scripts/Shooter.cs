using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Hero
{
    [Header("Параметры снаряда")]
    public float speedOfBullet;
    public float delayOfBullet;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", delayOfBullet, delayOfBullet);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Shoot()
    {
        var bullet = Instantiate(prefabBullet, prefabBody.transform.position, Quaternion.identity);
        bullet.GetComponent<BulletManager>().SetSpeed(speedOfBullet);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Hero
{
    [Header("Параметры снаряда")]
    public float speedOfBullet;
    public float firstDelayOfBullet;
    public float delayOfBullet;
    public float damage;

    // Start is called before the first frame update
    private void OnEnable()
    {
        InvokeRepeating("Shoot", firstDelayOfBullet, delayOfBullet);
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Shoot()
    {
        var bullet = Instantiate(prefabBullet, prefabBody.transform.position + new Vector3(0.5f, 0.75f, -5f), prefabBullet.transform.rotation);
        bullet.GetComponent<BulletManager>().SetSpeed(speedOfBullet);
        bullet.GetComponent<BulletManager>().SetDamage(damage);
    }
    private void OnDisable()
    {
        // Останавливаем все вызовы этого скрипта при отключении
        CancelInvoke();
    }
}

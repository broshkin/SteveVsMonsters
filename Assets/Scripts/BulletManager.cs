using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class BulletManager : MonoBehaviour
{
    private float bulletSpeed;
    private float damage;

    public enum EffectType
    {
        DEFAULT,
        FREEZED,
        BURNED,
        SLOWED,
    }

    public EffectType effectType = EffectType.DEFAULT;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(bulletSpeed * Time.deltaTime, 0, 0);
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
            if (effectType == EffectType.SLOWED)
            {
                other.GetComponent<Enemy>().StopSlowMoving();
                other.GetComponent<Enemy>().StartSlowMoving();
            }
            if (effectType == EffectType.FREEZED)
            {
                other.GetComponent<Enemy>().StopStopMoving();
                other.GetComponent<Enemy>().StartStopMoving();
            }
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().GetDamage(damage);
            if (effectType == EffectType.SLOWED)
            {
                other.GetComponent<Enemy>().StopSlowMoving();
                other.GetComponent<Enemy>().StartSlowMoving();
            }
            if (effectType == EffectType.FREEZED)
            {
                other.GetComponent<Enemy>().StopStopMoving();
                other.GetComponent<Enemy>().StartStopMoving();
            }
            Destroy(gameObject);
        }
    }

    public void SetDamage(float num) 
    {
        damage = num; 
    }
}

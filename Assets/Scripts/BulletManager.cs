using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private float bulletSpeed;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime);
    }

    public void SetSpeed(float num)
    {
        bulletSpeed = num;
    }
}

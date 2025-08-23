using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : Hero
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(Explode());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(1.5f);
        GetComponent<BoxCollider>().enabled = true;
        var a = GetComponentInChildren<ParticleSystem>();
        a.Play();
        a.transform.parent = null;
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }


}

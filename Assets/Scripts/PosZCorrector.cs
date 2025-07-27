using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PosZCorrector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.parent.localPosition.y - 5);
        }
            
    }
}

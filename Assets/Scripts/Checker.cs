using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public Line line;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (line != null)
        {
            if (!line.IsFree() && transform.parent != null)
            {
                if (TryGetComponent<Shooter>(out Shooter component_0))
                {
                    component_0.enabled = true;
                }
                GetComponent<Animator>().enabled = true;
            }
            else
            {
                if (TryGetComponent<Shooter>(out Shooter component_0))
                {
                    component_0.enabled = false;
                }
                GetComponent<Animator>().Rebind();
                GetComponent<Animator>().enabled = false;
            }
        }
    }
}

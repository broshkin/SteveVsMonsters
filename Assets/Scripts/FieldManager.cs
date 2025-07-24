using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{

    [SerializeField] private bool isFree;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            isFree = true;
        }
        else
        {
            isFree = false;
        }
    }

    public bool GetIsFree()
    {
        return isFree;
    }

    public void RemovePlant()
    {
        Destroy(transform.GetChild(0).gameObject);
    }
}

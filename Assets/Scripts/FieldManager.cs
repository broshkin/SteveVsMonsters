using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{

    [SerializeField] private bool isFree = true;
    public bool isZombieBossFree = true;
    private GameObject ZombieBoss;

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
        if (ZombieBoss != null)
        {
            isZombieBossFree = false;
        }
        else
        {
            isZombieBossFree = true;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && other.TryGetComponent<ZombieBoss>(out ZombieBoss comp1))
        {
            if (!comp1.inJumpAttack)
            {
                ZombieBoss = other.gameObject;
                isZombieBossFree = false;
            }
            
        }
    }
}

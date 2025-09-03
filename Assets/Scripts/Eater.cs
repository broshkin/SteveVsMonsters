using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : Hero
{
    [SerializeField] private bool isEating = false;
    [SerializeField] private bool once = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isEating && once)
        {
            once = false;
            StartCoroutine(Eating());
        }
        GetComponentInChildren<Animator>().SetBool("isEating", isEating);
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool GetIsEating()
    {
        return isEating;
    }
    public void SetIsEating(bool _isEating)
    {
        isEating = _isEating;
    }
    IEnumerator Eating()
    {
        yield return new WaitForSeconds(15);
        isEating = false;
        once = true;
    }
}

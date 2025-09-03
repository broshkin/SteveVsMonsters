using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomExpDrop : MonoBehaviour
{
    [SerializeField] private GameObject expPrefab;
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(Waiter());
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateExp()
    {
        var a = Instantiate(expPrefab, new Vector3(Random.Range(-4.5f, 4.5f), 4f, -15f), expPrefab.transform.rotation);
        a.GetComponent<ExpManager>().SetYOffset(Random.Range(-3f, 2.5f));
        a.GetComponent<ExpManager>().SetExpCount(25);
        a.GetComponent<Rigidbody>().mass = 0.01f;
    }

    IEnumerator Waiter()
    {
        yield return new WaitUntil(() => StartLevelButton.isStart);
        InvokeRepeating("GenerateExp", 5f, 10f);
    }
}

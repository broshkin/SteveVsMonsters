using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class Farmer : Hero
{
    [Header("Параметры роста")]
    public Material[] materials;
    public GameObject exp_prefab;
    public float grow_time;
    public int exp_count;
    [SerializeField]
    private bool ready_for_harvest = false;
    void Start()
    {
        StartCoroutine(Grow());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.transform.gameObject == gameObject && ready_for_harvest)
            {
                var exp = Instantiate(exp_prefab, transform.position - new Vector3(1, 0, 4), Quaternion.identity);
                exp.GetComponent<ExpManager>().SetExpCount(exp_count);
                exp.GetComponent<ExpManager>().SetYOffset(gameObject.transform.position.y);
                ready_for_harvest = false;
                StartCoroutine(Grow());
            }
        }
    }
    
    IEnumerator Grow()
    {
        ChangeMaterial(0);

        for (int i = 1; i < materials.Length; i++) 
        {
            yield return new WaitForSeconds(grow_time / (materials.Length - 1));
            ChangeMaterial(i);
        }

        ready_for_harvest = true;
    }

    public void ChangeMaterial(int i)
    {
        for (int j = 0; j < gameObject.transform.childCount; j++)
        {
            gameObject.transform.GetChild(j).GetComponent<MeshRenderer>().material = materials[i];
        }
    }
}

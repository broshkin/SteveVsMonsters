using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    private int exp_count;
    private float y_offset;
    private int[] plus_minus = { -1, 1 };

    [SerializeField] private LayerMask interactableLayer;
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 7 + Vector3.right * Random.Range(0.8f, 1.2f) * plus_minus[Random.Range(0, 2)], ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer) && hit.transform.gameObject == gameObject)
        {
            MoneySystem.AddMoney(exp_count);
            Destroy(gameObject);
        }

        if (gameObject.transform.position.y < y_offset)
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }

    }
    private void FixedUpdate()
    {
        transform.Rotate(0, 1.5f, 0);
    }
    public void SetExpCount(int num)
        { exp_count = num; }
    public void SetYOffset(float num)
        { y_offset = num; }
}

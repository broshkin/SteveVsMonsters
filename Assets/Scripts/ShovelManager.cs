using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;

public class ShovelManager : MonoBehaviour
{
    private bool on_hold = false;
    private bool on_field = false;
    private FieldManager field;
    private Vector3 start_pos;

    [SerializeField] private LayerMask interactableLayer;
    // Start is called before the first frame update
    void Start()
    {
        start_pos = GetComponent<RectTransform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition, null) && Input.GetMouseButtonDown(0))
        {
            var mousePos = Input.mousePosition;
            on_hold = true;
        }
        if (Input.GetMouseButton(0) && on_hold)
        {
            var mousePos = Input.mousePosition;
            Cursor.visible = false;
            gameObject.GetComponent<RectTransform>().position = mousePos;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                if (!hit.transform.gameObject.GetComponent<FieldManager>().GetIsFree())
                {
                    on_field = true;
                    field =  hit.transform.gameObject.GetComponent<FieldManager>();
                    Debug.Log(hit.transform.name);
                }
                else
                {
                    on_field = false;
                }
            }
            else
            {
                on_field = false;
            }
        }
        if (Input.GetMouseButtonUp(0) && on_hold && on_field)
        {
            field.RemovePlant();
            on_field = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.visible = true;
            on_hold = false;
            gameObject.GetComponent<RectTransform>().position = start_pos;
        }
    }
}

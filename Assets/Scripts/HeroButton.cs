using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HeroButton : MonoBehaviour
{
    public GameObject heroPrefab;
    public TextMeshProUGUI text;
    [SerializeField]
    private GameObject hero;
    [SerializeField]
    private GameObject field;
    [SerializeField]
    private bool OnField = false;

    [SerializeField] private LayerMask interactableLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition, null) && Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hero = Instantiate(heroPrefab, new Vector3(mousePos.x + 1, mousePos.y + 3, -1), heroPrefab.transform.rotation);

        }
        if (Input.GetMouseButton(0) && hero && MoneySystem.money >= hero.GetComponent<Hero>().cost)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer) && hit.transform.gameObject.tag == "Field")
            {
                if (hit.transform.gameObject.GetComponent<FieldManager>().GetIsFree())
                {
                    OnField = true;
                    field = hit.transform.gameObject;
                    if (hero.TryGetComponent<Shooter>(out Shooter component_0))
                    {
                        hero.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, -1);
                    }
                    if(hero.TryGetComponent<Farmer>(out Farmer component_1))
                    {
                        hero.transform.position = new Vector3(hit.transform.position.x + 1, hit.transform.position.y, -1);
                    }
                    Debug.Log(hit.transform.name);
                }
                else
                {
                    OnField = false;
                    field = null;
                    var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (hero.TryGetComponent<Shooter>(out Shooter component_0))
                    {
                        hero.transform.position = new Vector3(mousePos.x, mousePos.y + 3, -1);
                    }
                    if (hero.TryGetComponent<Farmer>(out Farmer component_1))
                    {
                        hero.transform.position = new Vector3(mousePos.x + 1, mousePos.y + 3, -1);
                    }
                    
                }
            }
            else
            {
                OnField = false;
                field = null;
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (hero.TryGetComponent<Shooter>(out Shooter component_0))
                {
                    hero.transform.position = new Vector3(mousePos.x, mousePos.y + 3, -1);
                }
                if (hero.TryGetComponent<Farmer>(out Farmer component_1))
                {
                    hero.transform.position = new Vector3(mousePos.x + 1, mousePos.y + 3, -1);
                }
            }
                
        }
        if (Input.GetMouseButtonUp(0) && hero && OnField)
        {
            if (field.GetComponent<FieldManager>().GetIsFree())
            {
                var heroOnField = Instantiate(heroPrefab, field.transform);

                if (heroOnField.TryGetComponent<Shooter>(out Shooter component_0))
                {
                    heroOnField.transform.position = new Vector3(field.transform.position.x, field.transform.position.y, -1);
                    heroOnField.transform.localScale /= 2;
                    component_0.enabled = true;
                }
                
                if (heroOnField.TryGetComponent<Farmer>(out Farmer component_1))
                {
                    heroOnField.transform.position = new Vector3(field.transform.position.x + 1, field.transform.position.y, -1);
                    heroOnField.transform.localScale /= 2;
                    component_1.enabled = true;
                }

                MoneySystem.RemoveMoney(heroOnField.GetComponent<Hero>().cost);
            }
            
            Destroy(hero);
        }
        else if (Input.GetMouseButtonUp(0) && hero)
        {
            Destroy(hero);
        }

        text.text = heroPrefab.GetComponent<Hero>().cost.ToString();
    }
} 

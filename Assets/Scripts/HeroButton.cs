using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

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

    
    private float initialSpeed = 100f;
    private float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;

    public StarsCounter starsCounter;

    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask UILayer;

    [Header("Õ≈ “–Œ√¿“‹!")]
    public ChooseHeroButton ChooseHeroButton;
    public int currentPosId;

    public void Start()
    {
        starsCounter = FindInActiveObjectByLayer(LayerMask.NameToLayer("StarsCounter")).GetComponent<StarsCounter>();
    }

    GameObject FindInActiveObjectByLayer(int layer)
    {

        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].gameObject.layer == layer)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

    void Update()
    {
        if (StartLevelButton.isStart)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition, null) && Input.GetMouseButtonDown(0) && MoneySystem.money >= heroPrefab.GetComponent<Hero>().cost)
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
                    if (hit.transform.gameObject.GetComponent<FieldManager>().GetIsFree() && hit.transform.gameObject.GetComponent<FieldManager>().isZombieBossFree)
                    {
                        OnField = true;
                        field = hit.transform.gameObject;
                        if (hero.TryGetComponent<Shooter>(out Shooter component_3))
                        {
                            hero.transform.position = Vector3.SmoothDamp(
                                hero.transform.position,
                                new Vector3(field.transform.position.x, field.transform.position.y - 0.45f, field.transform.position.z + field.transform.localPosition.y - 5),
                                ref velocity,
                                smoothTime,
                                initialSpeed
                            );
                        }
                        else if (hero.TryGetComponent<Farmer>(out Farmer component_4))
                        {
                            hero.transform.position = Vector3.SmoothDamp(
                                hero.transform.position,
                                new Vector3(field.transform.position.x + 0.5f, field.transform.position.y - 0.1f, field.transform.position.z + field.transform.localPosition.y - 5),
                                ref velocity,
                                smoothTime,
                                initialSpeed
                            );
                        }
                        else if (hero.TryGetComponent<Runner>(out Runner component_5))
                        {
                            hero.transform.position = Vector3.SmoothDamp(
                                hero.transform.position,
                                new Vector3(field.transform.position.x - 0.4f, field.transform.position.y - 0.45f, field.transform.position.z + field.transform.localPosition.y - 5),
                                ref velocity,
                                smoothTime,
                                initialSpeed
                            );
                        }
                        else if (hero.TryGetComponent<TNT>(out TNT component_6))
                        {
                            hero.transform.position = Vector3.SmoothDamp(
                                hero.transform.position,
                                new Vector3(field.transform.position.x - 0.2f, field.transform.position.y - 0.2f, field.transform.position.z + field.transform.localPosition.y - 5),
                                ref velocity,
                                smoothTime,
                                initialSpeed
                            );
                        }
                        else if (hero.TryGetComponent<Eater>(out Eater component_7))
                        {
                            hero.transform.position = Vector3.SmoothDamp(
                                hero.transform.position,
                                new Vector3(field.transform.position.x + 0.1f, field.transform.position.y - 0.2f, field.transform.position.z + field.transform.localPosition.y - 5),
                                ref velocity,
                                smoothTime,
                                initialSpeed
                            );
                        }
                        else
                        {
                            hero.transform.position = Vector3.SmoothDamp(
                               hero.transform.position,
                               new Vector3(field.transform.position.x, field.transform.position.y + 0.2f, field.transform.position.z + field.transform.localPosition.y - 5),
                               ref velocity,
                               smoothTime,
                               initialSpeed
                           );
                        }
                        //Debug.Log(hit.transform.name);
                    }
                    else
                    {
                        OnField = false;
                        field = null;
                        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        if (hero.TryGetComponent<Shooter>(out Shooter component_0))
                        {
                            hero.transform.position = new Vector3(mousePos.x, mousePos.y - 0.5f, -10);
                        }
                        else if (hero.TryGetComponent<Farmer>(out Farmer component_1))
                        {
                            hero.transform.position = new Vector3(mousePos.x + 0.5f, mousePos.y, -10);
                        }
                        else if (hero.TryGetComponent<Runner>(out Runner component_2))
                        {
                            hero.transform.position = new Vector3(mousePos.x, mousePos.y - 0.5f, -10);
                        }
                        else if (hero.TryGetComponent<TNT>(out TNT component_3))
                        {
                            hero.transform.position = new Vector3(mousePos.x - 0.2f, mousePos.y, -10);
                        }
                        else if (hero.TryGetComponent<Eater>(out Eater component_4))
                        {
                            hero.transform.position = new Vector3(mousePos.x, mousePos.y - 0.2f, -10);
                        }
                        else
                        {
                            hero.transform.position = new Vector3(mousePos.x, mousePos.y, -10);
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
                        hero.transform.position = new Vector3(mousePos.x, mousePos.y - 0.5f, -10);
                    }
                    else if (hero.TryGetComponent<Farmer>(out Farmer component_1))
                    {
                        hero.transform.position = new Vector3(mousePos.x + 0.5f, mousePos.y, -10);
                    }
                    else if (hero.TryGetComponent<Runner>(out Runner component_2))
                    {
                        hero.transform.position = new Vector3(mousePos.x, mousePos.y - 0.5f, -10);
                    }
                    else if (hero.TryGetComponent<TNT>(out TNT component_3))
                    {
                        hero.transform.position = new Vector3(mousePos.x - 0.2f, mousePos.y, -10);
                    }
                    else if (hero.TryGetComponent<Eater>(out Eater component_4))
                    {
                        hero.transform.position = new Vector3(mousePos.x, mousePos.y - 0.2f, -10);
                    }
                    else
                    {
                        hero.transform.position = new Vector3(mousePos.x, mousePos.y, -10);
                    }
                }

            }
            if (Input.GetMouseButtonUp(0) && hero && OnField)
            {
                if (field.GetComponent<FieldManager>().GetIsFree())
                {
                    starsCounter.currentHeroes++;
                    var heroOnField = Instantiate(heroPrefab, field.transform);
                    heroOnField.transform.localScale = new Vector3(heroOnField.transform.localScale.x / field.transform.lossyScale.x,
                        heroOnField.transform.localScale.y / field.transform.lossyScale.y, heroOnField.transform.localScale.z / field.transform.lossyScale.z);

                    if (heroOnField.TryGetComponent<Shooter>(out Shooter component_0))
                    {
                        heroOnField.transform.localPosition = new Vector3(0, -0.45f, 0);
                    }

                    if (heroOnField.TryGetComponent<Farmer>(out Farmer component_1))
                    {
                        heroOnField.transform.localPosition = new Vector3(0.5f, -0.1f, 0);
                        component_1.enabled = true;
                    }
                    if (heroOnField.TryGetComponent<Runner>(out Runner component_2))
                    {
                        heroOnField.transform.localPosition = new Vector3(0, -0.45f, -7.5f);
                        heroOnField.transform.parent = null;
                        heroOnField.GetComponent<BoxCollider>().enabled = true;
                    }
                    if (heroOnField.TryGetComponent<TNT>(out TNT copmonent_3))
                    {
                        heroOnField.transform.localPosition = new Vector3(-0.2f, -0.2f, -7.5f);
                        heroOnField.transform.parent = null;
                        copmonent_3.enabled = true;
                        heroOnField.GetComponentInChildren<Animator>().enabled = true;
                    }
                    if (heroOnField.TryGetComponent<Eater>(out Eater copmonent_4))
                    {
                        copmonent_4.enabled = true;
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
        else
        {
            if (ChooseHeroButton.isChoose)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition, null) && Input.GetMouseButtonDown(0))
                {
                    transform.parent.GetComponent<ActiveHeroesPanel>().freeWindows[currentPosId] = true;
                    transform.parent.GetComponent<ActiveHeroesPanel>().countOfHeroes--;
                    transform.parent = ChooseHeroButton.transform;
                    transform.localPosition = Vector3.zero;
                    transform.rotation = Quaternion.identity;
                    transform.localScale = new Vector3(1.36f, 1.36f, 1.36f);
                    currentPosId = -1;
                    ChooseHeroButton.isChoose = false;
                }
            }
        }
    }
      
} 

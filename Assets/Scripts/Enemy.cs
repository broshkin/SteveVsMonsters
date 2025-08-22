using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Основные параметры")]
    public float speed;
    public float hp;
    public float damage;
    public float damage_speed;

    private GameObject field;
    private bool isCoroutineRunning = false;
    private Animator anim;
    private bool isDie = false;

    [SerializeField] private LayerMask interactableLayer;

    public Line line;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.speed = speed / 1.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0 && !isDie)
        {
            isDie = true;
            line.MinusEnemy();
            GetComponent<BoxCollider>().enabled = false;
            anim.speed = 1;
            anim.SetBool("isDie", true);
            StartCoroutine(Dying());
        }

        Ray ray = new Ray(transform.position, Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10, interactableLayer) && !isDie)
        {
            if (hit.transform.gameObject.GetComponent<FieldManager>().GetIsFree())
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            else if (!isCoroutineRunning)
            {
                field = hit.transform.gameObject;
                if (!isCoroutineRunning && field.transform.childCount > 0)
                {
                    StartCoroutine(Eat());
                }
            }
        }
        else if (Physics.Raycast(ray, out hit, 10) && hit.transform.gameObject.tag == "Field" && !isDie)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        anim.SetBool("isEating", isCoroutineRunning && field.transform.childCount > 0);

        if (transform.position.x < -4.75f && !isDie)
        {
            GameLoopManager.lose = true;
        }
    }
    IEnumerator Eat()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(damage_speed);
        if (field.transform.childCount > 0)
        {
            field.transform.GetChild(0).gameObject.GetComponent<Hero>().GetDamage(damage);
        }
        isCoroutineRunning = false;
    }

    IEnumerator Dying()
    {
        print(123123123);
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }

    public void GetDamage(float num)
    {
        hp -= num;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Enemy : MonoBehaviour
{
    [Header("ќсновные параметры")]
    public float speed;
    private float slow_speed;
    private float basic_speed;
    public float hp;
    public float damage;
    public float damage_speed;

    private GameObject field;
    private bool isCoroutineRunning = false;
    private Animator anim;
    public bool isDie = false;
    private bool specialForEater = true;
    public enum EnemyType
    {
        ZOMBIE, // = 0 (по умолчанию нумераци€ с нул€)
        CREEPER, // = 1 
        ZOMBIE_BOSS, 
    }

    public EnemyType enemyType;
    [SerializeField] private LayerMask interactableLayer;
    public Line line;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.speed = speed / 1.25f;
        basic_speed = speed;
        slow_speed = speed / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0 && !isDie)
        {
            StopAllCoroutines();
            isDie = true;
            GetComponent<BoxCollider>().enabled = false;
            line.MinusEnemy();
            anim.speed = 1;
            anim.SetBool("isDie", true);
            StartCoroutine(Dying());
        }

        Ray ray = new Ray(transform.position, Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10, interactableLayer) && !isDie)
        {
            field = hit.transform.gameObject;
            if (hit.transform.gameObject.GetComponent<FieldManager>().GetIsFree() || !field.GetComponent<FieldManager>().isZombieBossFree)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            else if (!isCoroutineRunning)
            {
                if (!isCoroutineRunning && field.transform.childCount > 0 && field.GetComponent<FieldManager>().isZombieBossFree)
                {
                    if (enemyType == EnemyType.ZOMBIE)
                    {
                        StartCoroutine(Eat());
                    }
                    
                    if (enemyType == EnemyType.CREEPER)
                    {

                    }
                }
            }
        }
        else if (Physics.Raycast(ray, out hit, 10) && hit.transform.gameObject.tag == "Field" && !isDie)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (enemyType == EnemyType.ZOMBIE)
        {
            anim.SetBool("isEating", isCoroutineRunning && field.transform.childCount > 0);
        }

        if (enemyType == EnemyType.CREEPER)
        {

        }

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

    public IEnumerator Dying()
    {
        //print(123123123);
        speed = basic_speed;
        anim.speed = 1;
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }

    IEnumerator SlowMoving()
    {
        speed = slow_speed;
        anim.speed = slow_speed / 1.25f;
        yield return new WaitForSeconds(5f);
        speed = basic_speed;
        anim.speed = basic_speed / 1.25f;
    }
    IEnumerator StopMoving()
    {
        speed = 0;
        anim.speed = 0;
        yield return new WaitForSeconds(5f);
        speed = basic_speed;
        anim.speed = basic_speed / 1.25f;
    }
    public void StartSlowMoving()
    {
        StartCoroutine("SlowMoving");
    }
    public void StopSlowMoving()
    {
        StopCoroutine("SlowMoving");
    }

    public void StartStopMoving()
    {
        StartCoroutine(StopMoving());
    }
    public void StopStopMoving()
    {
        StopCoroutine(StopMoving());
    }

    public void GetDamage(float num)
    {
        hp -= num;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TNT" && !isDie)
        {
            GetDamage(200);
        }
        if (other.tag == "Eater")
        {
            if (!other.GetComponent<Eater>().GetIsEating())
            {
                StartCoroutine(WaitForEating(0.25f, other.gameObject));
                StartCoroutine(DyingNotFast(1));
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Eater")
        {
            if (!other.GetComponent<Eater>().GetIsEating() && specialForEater)
            {
                specialForEater = false;
                StartCoroutine(WaitForEating(0.25f, other.gameObject));
                StartCoroutine(DyingNotFast(1));
            }
        }
    }

    IEnumerator DyingNotFast(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    IEnumerator WaitForEating(float time, GameObject other)
    {
        yield return new WaitForSeconds(time);
        other.GetComponent<Eater>().SetIsEating(true);
    }
    
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ZombieBoss : Enemy
{
    // Start is called before the first frame update
    private Animator animator;
    private Vector3[] throwPos = {new Vector3(-1, 0.75f, -2), new Vector3(-1, -0.55f, -3), new Vector3(-1, -1.85f, -4) };
    public GameObject[] enemies;
    public BoxCollider needableCollider;
    public bool inJumpAttack = false;
    public GameObject hand;
    void Start()
    {
        for (int i = 0; i < GetComponents<BoxCollider>().Length; i++)
        {
            if (GetComponents<BoxCollider>()[i].center.x == -1.6f)
            {
                needableCollider = GetComponents<BoxCollider>()[i];
                break;
            }
        }

        needableCollider.enabled = false;
        animator = GetComponentInChildren<Animator>();
        
        StartCoroutine(ShakeCamera(0.8f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(ThrowZombie());
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(JumpAttack());
        }

        if (hp <= 0 && !isDie)
        {
            StopAllCoroutines();
            isDie = true;
            GetComponent<BoxCollider>().enabled = false;
            line.MinusEnemy();
            animator.speed = 1;
            animator.SetTrigger("Dying");
            StartCoroutine(Dying());
        }
    }

    IEnumerator ThrowZombie()
    {
        animator.SetTrigger("throwZombie");
        yield return new WaitForSeconds(0.93225f);
        var a = Instantiate(enemies[Random.Range(0, enemies.Length)], hand.transform);
        a.transform.localScale = new Vector3(0.0055f, 0.0055f, 0.0055f);
        a.GetComponent<Enemy>().enabled = false;
        a.GetComponent<BoxCollider>().enabled = false;
        a.transform.localPosition = new Vector3(-0.0142799998f, 0.0116400002f, -0.00553999981f);
        yield return new WaitForSeconds(0.339f);
        a.transform.parent = null;
        Vector3 randomPos = throwPos[Random.Range(0, throwPos.Length)];
        while (a.transform.position != randomPos || a.transform.rotation.eulerAngles != new Vector3(0, 0, 0))
        {
            a.transform.position = Vector3.MoveTowards(a.transform.position, randomPos, 0.1f);
            a.transform.rotation = Quaternion.Euler(Vector3.MoveTowards(a.transform.rotation.eulerAngles, new Vector3(0, 0, 0), 2f));
            yield return new WaitForEndOfFrame();
        }
        a.GetComponent<Enemy>().enabled = true;
        a.GetComponent<BoxCollider>().enabled = true;

    }

    IEnumerator JumpAttack()
    {
        animator.SetTrigger("jumpAttack");
        inJumpAttack = true;
        yield return new WaitForSeconds(2.25f);
        StartCoroutine(ShakeCamera(0));
        needableCollider.enabled = true;
        yield return new WaitForSeconds(0.25f);
        needableCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        inJumpAttack = false;
    }

    IEnumerator ShakeCamera(float time)
    {
        yield return new WaitForSeconds(time);
        Camera.main.GetComponent<Animator>().SetTrigger("ShakeCamera");
    }

    IEnumerator Dying()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("diying_zombie_boss"));
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        Destroy(gameObject); 
    }
}

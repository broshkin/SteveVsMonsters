using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;
using static UnityEditor.Progress;

public class LevelButton : MonoBehaviour
{
    public int starsNum;
    public int num;
    public bool isOpen;
    public GameObject zamok;
    public GameObject[] stars;
    // Start is called before the first frame update
    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(bool isLastOpenLevel)
    {
        Debug.Log((GetComponentInParent<Levels>().chapter - 1) * 10 + num);
        starsNum = GetComponentInParent<Levels>().storage.starsNums[(GetComponentInParent<Levels>().chapter - 1) * 10 + num];
        
        foreach (var item in stars)
        {
            item.GetComponentInChildren<Image>().sprite = item.GetComponentInParent<Levels>().unfillStar;
            Debug.Log(isOpen);
            item.GetComponentInChildren<Image>().gameObject.SetActive(isOpen);
        }

        if (starsNum > 3) starsNum = 3;

        for (int i = 0; i < starsNum; i++)
        {
            stars[i].GetComponentInChildren<Image>().sprite = stars[i].GetComponentInParent<Levels>().fillStar;
        }

        if (isLastOpenLevel)
        {
            foreach (var item in stars)
            {
                item.GetComponentInChildren<Image>().gameObject.SetActive(false);
            }

        }
    }
}

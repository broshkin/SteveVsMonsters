using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ActiveHeroesPanel : MonoBehaviour
{
    public float[] xOffsets = new float[6] { -309f, -185f, -64f, 62f, 188f, 312f};
    public bool[] freeWindows = new bool[6] { true, true, true, true, true, true };
    public int countOfHeroes = 0;
    void Start()
    {
        StartCoroutine(Waiter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Waiter()
    {
        yield return new WaitUntil(() => StartLevelButton.isStart);

    }
}

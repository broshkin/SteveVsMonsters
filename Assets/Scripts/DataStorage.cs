using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using YG;

public class DataStorage : MonoBehaviour
{
    // Start is called before the first frame update
    public int countLevelPasses;
    public int[] starsNums;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        YG2.onGetSDKData += GetData;
    }

    // Отписываемся от ивента onGetSDKData
    private void OnDisable()
    {
        YG2.onGetSDKData -= GetData;
    }

    private void Awake()
    {
        GetData();
    }

    public void SetData()
    {
        YG2.saves.countLevelPasses = countLevelPasses;
        YG2.saves.starsNums = starsNums;
    }

    public void GetData()
    {
        countLevelPasses = YG2.saves.countLevelPasses;
        starsNums = YG2.saves.starsNums;
    }

    public void Save()
    {
        SetData();
        YG2.SaveProgress();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMoneySystem : MonoBehaviour
{
    [SerializeField] private static int global_money = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMoney(int num)
    {
        global_money += num;
    }
    public void RemoveMoney(int num)
    {
        global_money -= num;
    }
    public int GetMoney()
    {
        return global_money;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneySystem : MonoBehaviour
{
    public static int money = 100;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        money = 100;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = money.ToString();
    }

    public static void AddMoney(int num)
    {
        money += num;
    }
    public static void RemoveMoney(int num)
    {
        money -= num;
    }
}

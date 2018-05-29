using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MoneyManager : MonoBehaviour {

    private static float money = 0;
    public static float Money
    {
        get
        {
            return money;
        }
        set
        {
            money = (float)Math.Round(value, 2);
        }
    }

    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }
    private void Update()
    {
        if (text)
        {
            text.text = "$" + money.ToString();
        }
    }

}

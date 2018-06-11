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

    public float starting_money;

    private void Start()
    {
        money = starting_money;
    }


}

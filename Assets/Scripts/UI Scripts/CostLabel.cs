using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CostLabel : MonoBehaviour {

    private Text text;
    private Placeable placeable;
    public GameObject Obj
    {
        set
        {
            placeable = value.GetComponent<Placeable>();
        }
    }

    private bool showing_low_funds = false;
    public float low_fund_time;
    public Color low_fund_color;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.transform.position = Input.mousePosition;

        text.text = "$" + Math.Round(placeable.cost, 2);
    }

    private void LateUpdate()
    {
        if (showing_low_funds)
        {
            text.color = low_fund_color;
        }
    }

    /// <summary>
    /// Flashes red briefly because you can't afford something
    /// </summary>
    public void LowFunds()
    {
        Debug.Log("color 1");
        showing_low_funds = true;
        Invoke("ResetColor", low_fund_time);
    }

    private void ResetColor()
    {
        showing_low_funds = false;
    }
}

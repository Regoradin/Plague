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

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.transform.position = Input.mousePosition;

        text.text = "$" + Math.Round(placeable.cost, 2);
    }
}

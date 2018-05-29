using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableWall : Placeable {

    private bool first_pos_set = false;
    private Vector3 first_pos;
    private Camera cam;
    private bool moused_up = false;

    public float cost_per_unit;

    private new void Awake()
    {
        base.Awake();
        cam = Camera.main;
    }

    public override void Build()
    {
        first_pos = transform.position;
        first_pos_set = true;
    }

    private void Update()
    {
        //This takes over the control of following mouse movement etc. from the ConstructionController after the first point has been chosen. Surely that won't cause any issues... right?
        if(first_pos_set)
        {
            if(!moused_up && Input.GetMouseButtonUp(0))
            {
                moused_up = true;
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            Vector3 second_pos = new Vector3(hit.point.x, 0, hit.point.z);

            transform.position = (first_pos + second_pos) / 2;
            float distance = Vector3.Distance(first_pos, second_pos);
            transform.localScale = new Vector3(distance, transform.localScale.y, transform.localScale.z);

            float angle = Mathf.Atan2(second_pos.z - first_pos.z, first_pos.x - second_pos.x);
            angle *= Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, angle);

            if (Input.GetMouseButtonDown(0) && moused_up && is_placeable)
            {
                cost = cost_per_unit * distance;
                base.Build();
            }
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(gameObject);
            }


        }

    }

}

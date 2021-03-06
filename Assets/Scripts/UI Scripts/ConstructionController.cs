﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionController : MonoBehaviour {

    private Camera cam;

    private GameObject obj;
    private bool bulldozing = false;

    public Texture2D build_cursor;
    public Texture2D bulldoze_cursor;
    public CostLabel cost_label;

	public float rot_inc;

    void Start()
    {
        cost_label.gameObject.SetActive(false);
        cam = Camera.main;

    }

    void Update()
    {
        if (obj || bulldozing)
        {
            int layer_mask = Physics.DefaultRaycastLayers;
            if (bulldozing)
            {
                //bulldozing needs to see things that typically ignore raycasts, because it needs to be able to see buildings
                layer_mask = layer_mask | (1 << 2);
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, maxDistance:Mathf.Infinity, layerMask:layer_mask);

            if (obj)
            {
                Cursor.SetCursor(build_cursor, Vector2.zero, CursorMode.Auto);

                obj.transform.position = new Vector3(hit.point.x, 0, hit.point.z);

                if (Input.GetMouseButtonDown(0))
                {
                    Build();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    cost_label.gameObject.SetActive(false);
                    Destroy(obj);
                }
				if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKeyDown("right"))
				{
					obj.transform.Rotate(Vector3.up * rot_inc);
				}
				if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKey("left"))
				{
					obj.transform.Rotate(Vector3.up * -rot_inc);
				}
            }
            if (bulldozing)
            {
                //sets hotspot to middle for bulldoze for now
                Cursor.SetCursor(bulldoze_cursor, Vector2.one * 24, CursorMode.Auto);

                if (Input.GetMouseButtonDown(0) && hit.collider)
                {
                    Destructible destruct = hit.collider.GetComponentInParent<Destructible>();
                    if (destruct)
                    {
                        destruct.Bulldoze();
						if (!Input.GetKey("left shift"))
						{
							bulldozing = false;
						}
                    }

                }
                if (Input.GetMouseButtonDown(1))
                {
                    bulldozing = false;
                }
            }
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

	public void Select(GameObject prefab)
	{
		if (!obj)
		{
			obj = Instantiate(prefab);
			bulldozing = false;
            cost_label.gameObject.SetActive(true);
            cost_label.Obj = obj;
        }
    }

    public void Bulldoze()
    {
        bulldozing = true;
        Destroy(obj);
    }

    private void Build()
    {
        Placeable placeable = obj.GetComponent<Placeable>();
        if (placeable.is_placeable && MoneyManager.Money >= placeable.cost)
        {
            //cost can't be subtracted from money here because it might change with wall type placeables.
            placeable.cost_label = cost_label;
            placeable.Build();
            obj = null;
        }
        else
        {
            cost_label.LowFunds();
        }
    }
}

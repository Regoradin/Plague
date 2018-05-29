using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionController : MonoBehaviour {

    private Camera cam;

    private GameObject obj;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (obj)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            obj.transform.position = new Vector3(hit.point.x, 0, hit.point.z);

            if (Input.GetMouseButtonDown(0))
            {
                Build();
            }
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(obj);
            }
        }
    }

    public void Select(GameObject prefab)
    {
        obj = Instantiate(prefab);
    }

    private void Build()
    {
        Placeable placeable = obj.GetComponent<Placeable>();
        if (placeable.is_placeable && MoneyManager.Money >= placeable.cost)
        {
            //cost can't be subtracted from money here because it might change with wall type placeables.
            placeable.Build();
            obj = null;
        }
    }
}

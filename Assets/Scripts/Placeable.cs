using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Placeable : MonoBehaviour {
    
    [HideInInspector]
    public bool is_placeable = true;
    private bool redder = false;

    public float cost;
    public float sell_value;

    public float redderness;
    public float alphaness;

    protected void Awake()
    {
        foreach(NavMeshObstacle obst in GetComponentsInChildren<NavMeshObstacle>())
        {
            obst.enabled = false;
        }
        foreach(Renderer rend in GetComponentsInChildren<Renderer>())
        {
            rend.material.color -= new Color(0, 0, 0, alphaness);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (is_placeable && other.tag != "Intersectable" && this.tag != "Intersectable")
        {
            is_placeable = false;
            if (!redder)
            {
                foreach (Renderer rend in GetComponentsInChildren<Renderer>())
                {
                    rend.material.color += new Color(redderness, 0, 0);
                    redder = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!is_placeable && other.tag != "Intersectable")
        {
            is_placeable = true;
            if (redder)
            {
                foreach (Renderer rend in GetComponentsInChildren<Renderer>())
                {
                    rend.material.color -= new Color(redderness, 0, 0);
                    redder = false;
                }
            }
        }
    }

    public virtual void Build()
    {
        if(MoneyManager.Money >= cost)
        {
            MoneyManager.Money -= cost;
        }
        else
        {
            return;
        }

        foreach (NavMeshObstacle obst in GetComponentsInChildren<NavMeshObstacle>())
        {
            obst.enabled = true;
        }
        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
        {
            rend.material.color += new Color(0, 0, 0, alphaness)
        }

		Destructible destruct = gameObject.AddComponent<Destructible>();
		destruct.sell_value = sell_value;
		Destroy(this);

    }

}

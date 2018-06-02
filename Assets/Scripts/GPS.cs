using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour {

	public GameObject park_parent;
	[HideInInspector]
    public List<Building> parks;

	private void Awake()
	{
		parks.AddRange(park_parent.GetComponentsInChildren<Building>());
	}
}

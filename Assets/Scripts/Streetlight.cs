using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Streetlight : MonoBehaviour {

	private Light light;

	private void Start()
	{
		light = GetComponent<Light>();
	}

	private void Update()
	{
		if (DayManager.is_day)
		{
			light.enabled = false;
		}
		else
		{
			light.enabled = true;
		}
	}

}

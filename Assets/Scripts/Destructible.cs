using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

	public float sell_value;

	public void Bulldoze()
	{
		MoneyManager.Money += sell_value;
		Destroy(gameObject);
	}

}

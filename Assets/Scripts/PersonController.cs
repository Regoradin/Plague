using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonController : MonoBehaviour {

	public GameObject dest_obj_setter;
	public static GameObject dest_obj;

	private void Update()
	{
		dest_obj = dest_obj_setter;
	}

}

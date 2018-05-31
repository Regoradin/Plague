using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalAttractor : MonoBehaviour {


	private void OnTriggerEnter(Collider other)
	{
		Person person = other.GetComponent<Person>();
		if (person)
		{
			if(person.disease > 0 && !person.Recently_cured)
			{
				person.nav_agent.SetDestination(transform.parent.GetComponent<Collider>().bounds.center);
			}
		}
	}

}

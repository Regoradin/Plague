using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Destination
	{
		work, home
	}

public class Building : MonoBehaviour {

	public float delay;

	public Destination destination;


	private void OnTriggerEnter(Collider other)
	{
		Person person = other.GetComponent<Person>();
		if (person)
		{
			if (person.dest_obj == gameObject)
			{
				person.nav_agent.enabled = false;

				Vector3 release_pos = person.transform.position;

				person.nav_agent.Warp(person.transform.position -= Vector3.up * 10);

				StartCoroutine(ReleasePerson(person, release_pos, delay));
			}
		}
	}

	private IEnumerator ReleasePerson(Person person, Vector3 release_pos, float delay)
	{
		yield return new WaitForSeconds(delay);
		person.nav_agent.enabled = true;

		person.nav_agent.Warp(release_pos);

		switch ((int)destination)
		{
			case 0:
				person.dest_obj = person.work;
				break;
			case 1:
				person.dest_obj = person.home;
				break;
			default:
				break;
		}
	}
}

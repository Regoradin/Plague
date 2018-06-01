using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkDelayer : MonoBehaviour {

	private List<Person> persons;
	public float delay;

	private bool processing = false;

	private void Awake()
	{
		persons = new List<Person>();
	}

	public void AddPerson(Person person)
	{
		if (!persons.Contains(person))
		{
			persons.Add(person);
		}
		if (!processing)
		{
			StartCoroutine(Process());
		}
	}

	private IEnumerator Process()
	{
		processing = true;
		while(persons.Count != 0)
		{
			persons[0].nav_agent.CompleteOffMeshLink();
			persons.Remove(persons[0]);
			yield return new WaitForSeconds(delay);
		}
		processing = false;
	}

}

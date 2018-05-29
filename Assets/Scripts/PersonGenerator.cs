using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonGenerator : MonoBehaviour {

	public GameObject person;
	public float delay;
	public List<Building> homes;
	public List<Building> works;

	private float last_time = 0;

    private bool has_infected = false;
	
	void Update () {
		if(Time.time >= last_time + delay)
		{
			last_time = Time.time;
			Person new_person = Instantiate(person, transform.position, Quaternion.identity).GetComponent<Person>();

			int i = Random.Range(0, homes.Count);
			new_person.home = homes[i];

			i = Random.Range(0, works.Count);
			new_person.work = works[i];

            if (!has_infected)
            {
                if(Random.Range(0, 2) == 1)
                {
                    Debug.Log("Infecting");
                    new_person.Infect();
                    has_infected = true;
                }
            }

		}
	}
}

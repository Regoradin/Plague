using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour {

	public GameObject homes_parent;
	public GameObject works_parent;
	private List<Building> homes;
	private List<Building> works;

	public GameObject person_prefab;
	public int population;
	public GameObject spawn_box;

	private void Start()
	{
		homes = new List<Building>();
		homes.AddRange(homes_parent.GetComponentsInChildren<Building>());
		works = new List<Building>();
		works.AddRange(works_parent.GetComponentsInChildren<Building>());

		for(int i  = 0; i < population; i++)
		{
			Vector3 position = new Vector3(Random.Range(-.5f, .5f), 0, Random.Range(-.5f, .5f));
			position = spawn_box.transform.TransformPoint(position);

			Person person = Instantiate(person_prefab, position, Quaternion.identity).GetComponent<Person>();

			int j = Random.Range(0, homes.Count);
			person.home = homes[j];

			j = Random.Range(0, works.Count);
			person.work = works[j];

		}
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Person : MonoBehaviour {

	public NavMeshAgent nav_agent;
	public GameObject dest_obj;

	public GameObject work;
	public GameObject home;

	void Start () {
		nav_agent = GetComponent<NavMeshAgent>();
		dest_obj = work;

		Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		color = new Color(1f, 1f, 1f);
		GetComponent<Renderer>().material.color = color;
	}
	
	void Update () {
		if (nav_agent.enabled)
		{
			nav_agent.SetDestination(dest_obj.transform.position);

		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Colliding");
		Color color_change = new Color(.1f, .1f, .1f);

		GetComponent<Renderer>().material.color -= color_change;

	}
}

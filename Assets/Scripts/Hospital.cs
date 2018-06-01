using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hospital : MonoBehaviour {

	public Transform inside;
	public Transform exit;

	public float cure_amount;
	public bool will_cure;
	public float min_to_cure;
	public float cost;
	public float wait_time;
	public float cure_cooldown;

	private List<Person> persons_waiting;
	private bool busy = false;

	private void Start()
	{
		persons_waiting = new List<Person>();
	}

	private void OnTriggerEnter(Collider other)
	{
		Person person = other.GetComponent<Person>();
		if (person && !person.Recently_cured)
		{
			person.nav_agent.enabled = false;
			person.nav_agent.Warp(inside.position);
			person.in_building = true;

			persons_waiting.Add(person);
			StartCoroutine(Cure(person));
		}
	}

	private IEnumerator Cure(Person person)
	{
		yield return new WaitUntil(() => persons_waiting[0] == person && !busy);
		yield return new WaitForSeconds(wait_time);

		if (MoneyManager.Money >= cost)
		{
			MoneyManager.Money -= cost;
			person.disease -= cure_amount;
			if (!will_cure && person.disease <= 0)
			{
				person.disease = person.disease_rate;
			}
			if (will_cure && person.disease <= min_to_cure)
			{
				person.disease = 0;
			}
		}
		persons_waiting.Remove(person);

		person.SetCureTimer(cure_cooldown);

		person.nav_agent.Warp(exit.position);
		person.nav_agent.enabled = true;
		person.in_building = false;
		person.ChooseDest();

	}
}

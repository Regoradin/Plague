using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Text money;
	public Text population;
	public Text infected;
	public Text deaths;

	private void Update()
	{
		money.text = "$" + MoneyManager.Money;

		population.text = "Population: " + PersonManager.population.Count;

		int infected_count = 0;
		foreach(Person person in PersonManager.population)
		{
			if(person.disease > 0)
			{
				infected_count += 1;
			}
		}
		infected.text = "Infected: " + infected_count;
		deaths.text = "Deaths: " + PersonManager.deaths;
	}

}

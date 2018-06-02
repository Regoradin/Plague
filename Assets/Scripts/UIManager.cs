using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Color day_text_color;
	public Color night_text_color;

	private List<Text> texts;

	public Text money;
	public Text population;
	public Text infected;
	public Text deaths;
	public Text days;

	private void Start()
	{
		texts = new List<Text> { money, population, infected, deaths, days };
	}

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
		days.text = "Day: " + DayManager.day;

		Color text_color = Color.black;
		if (DayManager.is_day)
		{
			text_color = day_text_color;
		}
		else
		{
			text_color = night_text_color;
		}
		foreach(Text text in texts)
		{
			text.color = text_color;
		}
	}

}

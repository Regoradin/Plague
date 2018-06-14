using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1 : MonoBehaviour {

	public GameObject homes_parent;
	public GameObject works_parent;
	private List<Building> homes;
	private List<Building> works;

	public GameObject person_prefab;
	public int population;
	public GameObject spawn_box;

    public int initial_infected;

    public GameObject loss_ui;
    public Text loss_text;
    public Text day_count;
    private bool lost = false;

    public void SetPopulation(string pop)
    {
        Debug.Log(pop);
        population = int.Parse(pop);
    }
    public void SetInitialInfected(string infected)
    {
        Debug.Log(infected);
        initial_infected = int.Parse(infected);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //reset statics
        MoneyManager.Money = 0;
        DayManager.day = 0;
        PersonManager.population = new List<Person>();
        PersonManager.deaths = 0;

    }

    private void Start()
    {
        loss_ui.SetActive(false);
    }
    private void Update()
    {
        //Lose Conditions
        if(MoneyManager.Money < 0)
        {
            Lose();
        }
    }
    private void Lose()
    {
        if (!lost)
        {
            lost = true;
            loss_ui.SetActive(true);
            day_count.text = "You lasted " + DayManager.day + " days.";

            StartCoroutine(FlashText(loss_text));
        }
    }
    private IEnumerator FlashText(Text text)
    {
        while (true)
        {
            text.color = Color.red;
            yield return new WaitForSeconds(2);
            text.color = Color.black;
            yield return new WaitForSeconds(2);
        }
    }
    
    public void StartLevel()
	{
		homes = new List<Building>();
		homes.AddRange(homes_parent.GetComponentsInChildren<Building>());
		works = new List<Building>();
		works.AddRange(works_parent.GetComponentsInChildren<Building>());

        //converts building capacities from percentage of population to actual number
        foreach(Building building in homes)
        {
            building.capacity *= population;
        }
        foreach(Building building in works)
        {
            building.capacity *= population;
        }

		for(int i  = 0; i < population; i++)
		{

			Vector3 position = new Vector3(Random.Range(-.5f, .5f), 0, Random.Range(-.5f, .5f));
			position = spawn_box.transform.TransformPoint(position);

			Person person = Instantiate(person_prefab, position, Quaternion.identity).GetComponent<Person>();

            bool found_home = false;
            while (!found_home)
            {
                int j = Random.Range(0, homes.Count);
                if (homes[j].capacity > 0)
                {
                    found_home = true;
                    person.home = homes[j];
                    homes[j].capacity--;
                }
            }

            bool found_work = false;
            while (!found_work)
            {
                int j = Random.Range(0, works.Count);
                if (works[j].capacity > 0)
                {
                    found_work = true;
                    person.work = works[j];
                    works[j].capacity--;
                }
            }


            if (i < initial_infected)
            {
                person.Infect();
                person.name = "INFECTED PERSON";
            }

        }
	}

}

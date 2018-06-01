using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Person : MonoBehaviour {

    private Renderer rend;
    private GPS gps;
	[HideInInspector]
	public NavMeshAgent nav_agent;

	[Header("Navigation Info")]
    public Building dest_building;
	public Building work;
	public Building home;
	public bool in_building = false;

	[Header("Disease Info")]
    public float disease = 0;
    public float disease_rate = .01f;

    public float infection_rate = .5f;
    public float cough_radius;

    public GameObject corpse;

    private float happy;
    private float sleep;

    private float happy_thresh_low;
    private float happy_thresh_high;
    private float sleep_thresh_low;
    private float sleep_thresh_high;

    public float ambient_sleep_loss;

	public bool recently_cured;
	public bool Recently_cured
	{
		get
		{
			return recently_cured;
		}
	}
	public void SetCureTimer(float time)
	{
		StartCoroutine(CureTimer(time));
	}
	private  IEnumerator CureTimer(float time)
	{
		recently_cured = true;
		yield return new WaitForSeconds(time);
		recently_cured = false;
	}

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        gps = GameObject.Find("GPS").GetComponent<GPS>();
		PersonManager.population.Add(this);

        nav_agent = GetComponent<NavMeshAgent>();

        rend.material.color = new Color(1f, 1f, 1f);

        int plagueMask = NavMesh.GetAreaFromName("Plague");
        int healthyMask = NavMesh.GetAreaFromName("Healthy");
        nav_agent.areaMask = nav_agent.areaMask | (1 << healthyMask);
        nav_agent.areaMask = nav_agent.areaMask & ~(1 << plagueMask);

        //Set initial happy and sleep values and thresholds.
        happy = .1f;
        happy_thresh_low = .2f + Random.Range(-.19f, .2f);
        happy_thresh_high = .5f + Random.Range(-.1f, .1f);
        if(happy_thresh_low >= happy_thresh_high)
        {
            happy_thresh_low = happy_thresh_high - .1f;
        }
        sleep = 1;
        sleep_thresh_low = .2f + Random.Range(-.1f, .1f);
        sleep_thresh_high = .6f + Random.Range(0f, .3f);

        StartCoroutine(BeAwake());
    }

    private void Start()
    {
        foreach (Cough cough in GetComponentsInChildren<Cough>())
        {
            cough.Radius = cough_radius;
        }
        ChooseDest();
    }
    
    /// <summary>
    /// Chooses the destination as home if tired, a park if sad, and work otherwise.
    /// </summary>
    public void ChooseDest()
    {
        int i = Random.Range(0, work.doors.Count);
        GameObject dest = work.doors[i];
        dest_building = work;

        if(sleep < sleep_thresh_low)
        {
            int j = Random.Range(0, home.doors.Count);
            dest = home.doors[j];
            dest_building = home;
        }
        else if (happy < happy_thresh_low)
        {
            int j = Random.Range(0, gps.parks.Count);
            dest = gps.parks[j].doors[0];
            dest_building = gps.parks[j];
        }

        nav_agent.SetDestination(dest.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dest_building.doors.Contains(other.gameObject))
        {
            StartCoroutine(DoBuilding());
        }
    }

    private IEnumerator DoBuilding()
    {
        //Enter Building
        nav_agent.enabled = false;
        nav_agent.Warp(dest_building.inside.position);
		in_building = true;

        //Do things in building
        if(dest_building == work)
        {
            while(happy > happy_thresh_low && sleep > sleep_thresh_low)
            {
                MoneyManager.Money += dest_building.income;
                sleep += dest_building.sleep;
                happy += dest_building.happy;

                yield return new WaitForSeconds(1f);
            }
        }
        else if(dest_building == home)
        {
            while(sleep < sleep_thresh_high)
            {
                MoneyManager.Money += dest_building.income;
                sleep += dest_building.sleep;
                happy += dest_building.happy;

                yield return new WaitForSeconds(1f);
            }
        }
        else
        {
            while(happy < happy_thresh_high && sleep > sleep_thresh_low)
            {
                MoneyManager.Money += dest_building.income;
                sleep += dest_building.sleep;
                happy += dest_building.happy;

                yield return new WaitForSeconds(1f);
            }
        }

        //Leave Building
        int i = Random.Range(0, dest_building.doors.Count);
        nav_agent.Warp(dest_building.doors[i].transform.position);
        nav_agent.enabled = true;
		in_building = false;

        ChooseDest();

    }

    private IEnumerator BeAwake()
    {
        while (true)
        {
            sleep -= ambient_sleep_loss;
            yield return new WaitForSeconds(1);
        }
    }

	private void Update()
	{
		if (nav_agent.isOnOffMeshLink)
		{
			nav_agent.currentOffMeshLinkData.offMeshLink.GetComponent<LinkDelayer>().AddPerson(this);
		}
	}

	/// <summary>
	/// To be called on people who have been coughed upon by some other person.
	/// </summary>
	/// <param name="other">The person inflicting the cough.</param>
	public void CoughedUpon(Person other)
    {
        if (disease == 0 && other.disease > 0)
        {
            if (Random.Range(0f, 1f) < other.infection_rate)
            {
                StartCoroutine(Disease());
            }
        }
    }
    public void CoughedUpon(Corpse other)
    {
        if(disease == 0 && !nav_agent.isOnOffMeshLink)
        {
            StartCoroutine(Disease());
        }
    }

    private IEnumerator Disease()
    {
        //sets navigation mask to plague and not healthy
        int plagueMask = NavMesh.GetAreaFromName("Plague");
        int healthyMask = NavMesh.GetAreaFromName("Healthy");
        nav_agent.areaMask = nav_agent.areaMask | (1 << plagueMask);
        nav_agent.areaMask = nav_agent.areaMask & ~(1 << healthyMask);

		//initial increase of disease so that it won't be treated as cured
		disease += disease_rate;

        while (disease < 1 && disease > 0)
        {
            disease += disease_rate;
            rend.material.color = new Color(1f - disease, 1f - (disease/3), 1f - disease);

            yield return new WaitForSeconds(1);
        }

		if (disease >= 1)
		{
			Die();
		}
    }

    public void Die()
    {
        Destroy(gameObject);

		PersonManager.population.Remove(this);
		PersonManager.deaths++;
		if (in_building)
		{
			Debug.Log("Dieing in building");
			//prevents corpses from piling up in the nether
			int i = Random.Range(0, dest_building.doors.Count);
			transform.position = dest_building.doors[i].transform.position;
		}
        Instantiate(corpse, new Vector3(transform.position.x, 0, transform.position.z) , Quaternion.Euler(90, 0, Random.Range(0, 360)));
    }

    public void Infect()
    {
        StartCoroutine(Disease());
    }
}

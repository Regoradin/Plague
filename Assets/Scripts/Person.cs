using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Person : MonoBehaviour {

    private Renderer rend;

	public NavMeshAgent nav_agent;
	public GameObject dest_obj;

	public GameObject work;
	public GameObject home;

    public float disease = 0;
    public float disease_rate = .01f;

    public float infection_rate = .5f;
    public float cough_radius;

    public GameObject corpse;

    private void Awake()
    {
        rend = GetComponent<Renderer>();

        nav_agent = GetComponent<NavMeshAgent>();

        rend.material.color = new Color(1f, 1f, 1f);

        int plagueMask = NavMesh.GetAreaFromName("Plague");
        int healthyMask = NavMesh.GetAreaFromName("Healthy");
        nav_agent.areaMask = nav_agent.areaMask | (1 << healthyMask);
        nav_agent.areaMask = nav_agent.areaMask & ~(1 << plagueMask);


    }

    private void Start()
    {
        dest_obj = work;

        foreach (Cough cough in GetComponentsInChildren<Cough>())
        {
            cough.Radius = cough_radius;
        }
    }

    void Update()
    {
        if (nav_agent.enabled)
        {
            nav_agent.SetDestination(dest_obj.transform.position);
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
        if(disease == 0)
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

        while (disease < 1)
        {
            disease += disease_rate;
            rend.material.color = new Color(1f - disease, 1f - (disease/3), 1f - disease);

            yield return new WaitForSeconds(1);
        }

        Die();
    }

    public void Die()
    {
        Debug.Log("Dieing");
        Destroy(gameObject);

        Instantiate(corpse, new Vector3(transform.position.x, 0, transform.position.z) , Quaternion.Euler(90, 0, Random.Range(0, 360)));
    }

    public void Infect()
    {
        StartCoroutine(Disease());
    }
}

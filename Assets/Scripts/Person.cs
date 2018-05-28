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

    private float disease = 0;
    public float disease_rate = .01f;

    public float infection_rate = .5f;
    public float cough_radius;

    public GameObject corpse;

    private void Start () {
        rend = GetComponent<Renderer>();

        nav_agent = GetComponent<NavMeshAgent>();
		dest_obj = work;

		rend.material.color = new Color(1f, 1f, 1f);

        foreach(CapsuleCollider collider in GetComponentsInChildren<CapsuleCollider>())
        {
            if (collider.GetComponent<Cough>())
            {
                collider.radius = cough_radius;
            }
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

    public void GIVE_DISEASE()
    {
        StartCoroutine(Disease());
    }
}

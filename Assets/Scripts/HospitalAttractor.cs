using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalAttractor : MonoBehaviour {


    private Collider parent_coll;

    private void Start()
    {
        parent_coll = transform.parent.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
	{
        if (enabled)
        {
            Person person = other.GetComponent<Person>();
            if (person && !person.Recently_cured)
            {
                if (person.disease > 0)
                {
                    person.nav_agent.SetDestination(parent_coll.bounds.center);
                    person.going_to_hospital = true;
                }
            }
        }
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cough : MonoBehaviour {

    public Person person;
    private CapsuleCollider coll;
    private ParticleSystem particle;
    public float delay;

    private float radius;
    public float Radius
    {
        get
        {
            return radius;
        }
        set
        {
            radius = value;
            coll.radius = radius;

            var main = particle.main;
            main.startSpeed = (radius / 2) - 1;
        }
    }

    private void Awake()
    {
        person = GetComponentInParent<Person>();
        coll = GetComponent<CapsuleCollider>();
        particle = GetComponent<ParticleSystem>();

        StartCoroutine(ToggleCollider());
    }

    private IEnumerator ToggleCollider()
    {
        while (true)
        {
            if (coll.enabled)
            {
                coll.enabled = false;
                yield return new WaitForSeconds(delay);
            }
            else
            {
                coll.enabled = true;
                if (person.disease > 0)
                {
                    particle.Play();
                }
                //this ensures that it is only enabled for one physics frame, and thus only coughs on people once. 
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Cough on the other person, as long as they are in fact a person.
        Person other_person = other.GetComponent<Person>();

        if (other_person)
        {
            other_person.CoughedUpon(person);
        }
    }

}

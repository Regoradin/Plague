using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cough : MonoBehaviour {

    public Person person;
    private Collider coll;
    public float delay;

    private void Start()
    {
        person = GetComponentInParent<Person>();
        coll = GetComponent<Collider>();

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

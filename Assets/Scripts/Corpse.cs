using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Person other_person = other.GetComponent<Person>();
        if (other_person)
        {
            other_person.CoughedUpon(this);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

	public Transform inside;
	public int capacity;
    public List<GameObject> doors;

    [Header("Change Per Second")]
    public float income;
    public float happy;
    public float sleep;
}

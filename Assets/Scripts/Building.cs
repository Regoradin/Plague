using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    public List<GameObject> doors;
    public Transform inside;

    [Header("Change Per Second")]
    public float income;
    public float happy;
    public float sleep;
}

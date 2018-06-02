using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour {

	public float day_length;
	public Light sun;

	public static int day = 1;
	public static bool is_day = true;

	//sunrise is at x = 180, sunset is at x = 0, and x is rotated negatively for time to move forward
	private void Start()
	{
		sun.transform.eulerAngles = new Vector3(180, 90, 0);
		StartCoroutine(CountDays());
		StartCoroutine(CycleIsDay());
	}

	private void Update()
	{
		sun.transform.Rotate(Vector3.left * (360/(day_length) * Time.deltaTime));
	}

	private IEnumerator CountDays()
	{
		while (true)
		{
			yield return new WaitForSeconds(day_length);
			day++;
		}
	}
	private IEnumerator CycleIsDay()
	{
		while (true)
		{
			yield return new WaitForSeconds(day_length / 2);
			is_day = !is_day;
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour 
{
	public GameObject toSpawn;
	public StatusController current;
	public float timeToSpawn = 5f;
	private float currentTime;

	private void Start()
	{
		
	}
	private void Update()
	{
		if(current == null)
		{
			if(Time.time >= currentTime)
			{
				current = Instantiate(toSpawn, transform.position, Quaternion.identity).GetComponent<StatusController>();
			}
		}
		else
		{
			if(current.isDying)
			{
				currentTime = Time.time + timeToSpawn;
				current = null;
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAddForce : MonoBehaviour 
{
	private void Start()
	{
		Invoke("Add", 3f);
	}

	public void Add()
	{
		GetComponent<Rigidbody>().AddForce(-transform.forward * 5, ForceMode.VelocityChange);
		Debug.Log("Add");
	}
}

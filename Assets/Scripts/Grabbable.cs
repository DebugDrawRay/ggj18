using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour 
{
	public Rigidbody rigid
	{
		get;
		private set;
	}
	
	public float maxMag = 10;
	private bool draining;

	private Vector3 direction;
	public float magnitude
	{
		get;
		private set;
	}
	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	public bool adding = true;
	public void ChangeVelocity(float amount)
	{
		if(!draining)
		{
			direction = rigid.velocity.normalized;
			if(direction == Vector3.zero)
			{
				direction = (PlayerController.position - transform.position).normalized;
			}
			magnitude = rigid.velocity.magnitude;
			rigid.velocity = Vector3.zero;
			draining = true;
		}
		magnitude -= amount;
		magnitude = Mathf.Clamp(magnitude, -maxMag, maxMag);
		adding = magnitude < 0;
	}

	public void StopDrain()
	{
		if(magnitude < 0)
		{
			direction = PlayerController.position - transform.position;
		}
		rigid.velocity = direction * magnitude;
		draining = false;
	}
}

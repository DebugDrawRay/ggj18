using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour 
{
	private Rigidbody rigid;
	
	private bool draining;

	private Vector3 direction;
	private float magnitude;
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
				direction = PlayerController.instance.gameObject.transform.position - transform.position;
				direction = direction.normalized;
			}
			magnitude = rigid.velocity.magnitude;
			rigid.velocity = Vector3.zero;
			draining = true;
		}
		magnitude -= amount;
		adding = magnitude < 0;
	}

	public void StopDrain()
	{
		rigid.velocity = direction * magnitude;
		draining = false;
	}
}

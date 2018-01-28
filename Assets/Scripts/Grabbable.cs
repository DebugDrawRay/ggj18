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
	private bool changing;

	private Vector3 direction;

	public bool aggressive = true;
	public float magnitude
	{
		get;
		private set;
	}
	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	public void StartChange(Transform newParent)
	{
		direction = rigid.velocity.normalized;
		magnitude = rigid.velocity.magnitude;

		rigid.velocity = Vector3.zero;
		rigid.isKinematic = true;

		transform.SetParent(newParent);
		transform.localPosition = Vector3.zero;

		changing = true;
	}
	public void ChangeVelocity(float amount)
	{
		magnitude -= amount;
		magnitude = Mathf.Clamp(magnitude, -maxMag, maxMag);
	}

	public void StopChange()
	{
		rigid.isKinematic = false;
		transform.SetParent(null);

		if(magnitude < 0)
		{
			gameObject.layer = LayerMask.NameToLayer("GrabbableFromPlayer");
			direction = -PlayerController.instance.GetComponent<StatusController>().CurrentFacing;
			aggressive = false;
		}
		rigid.velocity = direction * magnitude;

		magnitude = 0;
		direction = Vector3.zero;
	}
	public void SetMagnitude(float amount)
	{
		magnitude = amount;
	}
	private void OnCollisionEnter(Collision other)
	{
		aggressive = false;
		rigid.velocity = Vector3.zero;
	}
}

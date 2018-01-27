using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityGrabber : ControlAction 
{
	public Grabber grabber;
	public float grabOffset = 1f;
	private StatusController status;

	public float drainAmount;
	private bool hasStopped = true;
	public float storedVelocity;
	public float maxVelocity = 30;
	protected override void Awake()
	{
		base.Awake();
		status = GetComponent<StatusController>();
	}
	private void Start()
	{
		UIController.instance.UpdateVelocity(storedVelocity/ maxVelocity);
	}
	public override void UpdateAction(ActionSet actions)
	{
		if(actions.primaryAction)
		{
			if(grabber.currentObject != null)
			{
				hasStopped = false;
				Grabbable current = grabber.currentObject;
				if(current.adding)
				{
					if(storedVelocity > 0)
					{
						storedVelocity -= drainAmount;
						current.ChangeVelocity(drainAmount);
					}
					status.arrow.transform.localRotation = Quaternion.Euler(new Vector3(0,0,180));
				}
				else
				{
					if(storedVelocity < Mathf.Infinity)
					{
						storedVelocity += drainAmount;
						current.ChangeVelocity(drainAmount);
					}
					status.arrow.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
				}
				storedVelocity = Mathf.Clamp(storedVelocity, 0, maxVelocity);
				status.arrow.fillAmount = Mathf.Abs(current.magnitude / current.maxMag);
				UIController.instance.UpdateVelocity(storedVelocity/ maxVelocity);
			}
		}
		else
		{
			if(!hasStopped)
			{
				if(grabber.currentObject != null)
				{
					grabber.currentObject.StopDrain();
				}
				hasStopped = true;
			}
		}
		status.arrowCanvas.gameObject.SetActive(actions.primaryAction);
		GetComponent<TwoAxisMovement>().canMove = !actions.primaryAction;
	}
	private void Update()
	{
		grabber.transform.position = transform.position + (status.CurrentFacing * grabOffset);
	}
}

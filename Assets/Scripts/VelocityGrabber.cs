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
	protected override void Awake()
	{
		base.Awake();
		status = GetComponent<StatusController>();
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
				}
				else
				{
					if(storedVelocity < Mathf.Infinity)
					{
						storedVelocity += drainAmount;
						current.ChangeVelocity(drainAmount);
					}
				}
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

		GetComponent<TwoAxisMovement>().canMove = !actions.primaryAction;
	}
	private void Update()
	{
		grabber.transform.position = transform.position + (status.CurrentFacing * grabOffset);
	}
}

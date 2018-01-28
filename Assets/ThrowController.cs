using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : ControlAction 
{
	public GameObject throwable;
	private Grabbable current;
	public float throwableOffset;
	public float throwForce;
	private StatusController status;
	protected override void Awake()
	{
		base.Awake();
		status = GetComponent<StatusController>();
	}

	public override void UpdateAction(ActionSet actions)
	{
		if(actions.primaryAction)
		{
			Prepare();
		}
		else
		{
			Throw();
		}
	}

	private void Update()
	{
		if(current)
		{
			current.transform.position = transform.position - (status.GetFacingCardinal(status.CurrentFacing) * throwableOffset);
		}
	}
	public void Prepare()
	{
		if(current == null)
		{
			Vector3 origin = transform.position - (status.GetFacingCardinal(status.CurrentFacing) * throwableOffset);
			current = Instantiate(throwable, origin, Quaternion.identity, transform).GetComponent<Grabbable>();
		}
	}

	public void Throw()
	{
		if(current != null)
		{
			current.transform.SetParent(null);
			current.rigid.isKinematic = false;
			Vector3 force = status.CurrentFacing * throwForce;
			current.rigid.AddForce(force, ForceMode.Impulse);
			current.isEnemies = false;
			current = null;
		}
	}
}

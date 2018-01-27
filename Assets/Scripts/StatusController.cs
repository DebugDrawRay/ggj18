using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour 
{
	private Vector3 currentFacing = Vector3.forward;
	public Vector3 CurrentFacing
	{
		get
		{
			return currentFacing;
		}
		set
		{
			if(Mathf.Abs(value.x) > Mathf.Abs(value.z))
			{
				value.z = 0;
				value.x = Mathf.Round(value.x);
			}
			else
			{
				value.x = 0;
				value.z = Mathf.Round(value.z);
			}
			currentFacing = value;
		}
	}

	public float currentVelocity;
}

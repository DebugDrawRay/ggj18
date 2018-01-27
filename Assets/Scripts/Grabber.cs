using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour 
{
	public Grabbable currentObject;
	public bool isDraining;
	private void OnTriggerEnter(Collider other)
	{
		if(!isDraining)
		{
			if(currentObject == null)
			{
				currentObject = other.GetComponent<Grabbable>();
			}
			else
			{
				float currentDist = Vector3.Distance(transform.parent.position, currentObject.transform.position);
				float newDist = Vector3.Distance(transform.parent.position, other.transform.position);
				if(newDist < currentDist)
				{
					currentObject = other.GetComponent<Grabbable>();
				}
			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if(!isDraining)
		{
			if(currentObject != null && currentObject.Equals(other.GetComponent<Grabbable>()))
			{
				currentObject = null;
			}
		}
	}
}

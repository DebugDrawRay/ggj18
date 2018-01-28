using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public Grabbable currentObject;
    public bool isDraining;
    public GameObject visual;
    public GameObject holder;
	public List<Grabbable> currentObjects = new List<Grabbable>();
    private void OnTriggerEnter(Collider other)
    {
		if(other.GetComponent<Grabbable>() && !currentObjects.Contains(other.GetComponent<Grabbable>()))
		{
			currentObjects.Add(other.GetComponent<Grabbable>());
			other.GetComponent<Grabbable>().StartChange(holder.transform);
		}
        // if (currentObject == null)
        // {
        //     currentObject = other.GetComponent<Grabbable>();
        // }
        // else
        // {
        //     float currentDist = Vector3.Distance(transform.parent.position, currentObject.transform.position);
        //     float newDist = Vector3.Distance(transform.parent.position, other.transform.position);
        //     if (newDist < currentDist)
        //     {
        //         if (!isDraining)
        //         {
        //             currentObject = other.GetComponent<Grabbable>();
        //         }
        //     }
        // }
    }

    private void OnTriggerExit(Collider other)
    {
		if(currentObjects.Contains(other.GetComponent<Grabbable>()))
		{
			currentObjects.Remove(other.GetComponent<Grabbable>());
			other.GetComponent<Grabbable>().StopChange();
		}
        // if (currentObject != null && currentObject.Equals(other.GetComponent<Grabbable>()))
        // {
        //     currentObject.StopChange();
        //     currentObject = null;
        // }
    }
}
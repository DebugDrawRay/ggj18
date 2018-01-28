using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour 
{
	public AudioClip destroy;

	private void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.GetComponent<Grabbable>() && other.gameObject.layer == LayerMask.NameToLayer("GrabbableFromPlayer"))
		{
			AudioManager.instance.PlaySfx(destroy);
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}
}

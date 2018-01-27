using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAction : MonoBehaviour 
{
	public bool fixedUpdate;

	protected virtual void Awake()
	{
		GetComponent<InputBus>().Subscribe(UpdateAction, fixedUpdate);
	}
	private void OnDestroy()
	{
		GetComponent<InputBus>().Unsubscribe(UpdateAction, fixedUpdate);
	}
	public virtual void UpdateAction(ActionSet actions) { }
}

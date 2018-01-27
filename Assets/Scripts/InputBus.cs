using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBus : MonoBehaviour 
{
	private IInputController input;

	public delegate void ActionDelegate(ActionSet actions);
	public ActionDelegate UpdateActions;
	public ActionDelegate UpdateFixedActions;

	private void Awake()
	{
		input = GetComponent<IInputController>();
	}

	public void Subscribe(ActionDelegate action, bool fixedUpdate)
	{
		if(fixedUpdate)
		{
			UpdateFixedActions += action;
		}
		else
		{
			UpdateActions += action;
		}
	}
	public void Unsubscribe(ActionDelegate action, bool fixedUpdate)
	{
		if(fixedUpdate)
		{
			UpdateFixedActions -= action;
		}
		else
		{
			UpdateActions -= action;
		}
	}

	private void Update()
	{
		if(UpdateActions != null)
		{
			UpdateActions(input.Actions);
		}
	}
	private void FixedUpdate()
	{
		if(UpdateFixedActions != null)
		{
			UpdateFixedActions(input.Actions);
		}
	}
}

public class ActionSet
{
	public Vector3 moveVector;
	public bool primaryAction;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour, IInputController
{
	public static PlayerController instance;
	public static Vector3 position;
	public ActionSet Actions
	{
		get;
		set;
	}
	public Player input;
	private void Awake()
	{
		Actions = new ActionSet();
		input = ReInput.players.GetPlayer(0);
		instance = this;
	}
	private void Update()
	{
		Vector2 move = input.GetAxis2D("moveX", "moveY");
		Actions.moveVector.x = move.x;
		Actions.moveVector.z = move.y;
		Actions.primaryAction = input.GetButton("grab");

		position = transform.position;
	}
}


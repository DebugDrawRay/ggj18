using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour, IInputController
{
	public static PlayerController instance;
	public static Vector3 position;
	public int collectables;
	public AudioClip collect;
	private int target = 3;
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

	private void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.GetComponent<Collectable>())
		{
			Destroy(other.gameObject);
			collectables++;
			UIController.instance.UpdateCollectables(collectables);
			AudioManager.instance.PlaySfx(collect);
		}
	}
}


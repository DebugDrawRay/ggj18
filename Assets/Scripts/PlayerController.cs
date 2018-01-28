using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour, IInputController
{
	public static PlayerController instance;
	public static Vector3 position;
	public int collectables;
	public AudioClip collect;
	public AudioClip foundAll;
	private int target = 3;
	private bool ending;
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
		GetComponent<Rigidbody>().velocity = Vector3.zero;

		Vector2 move = input.GetAxis2D("moveX", "moveY");
		Actions.moveVector.x = move.x;
		Actions.moveVector.z = move.y;
		Actions.primaryAction = input.GetButton("grab");

		position = transform.position;

		if(collectables >= 3&& !ending)
		{
			Invoke("Win", 1.5f);
			AudioManager.instance.PlaySfx(foundAll);
			ending = true;
		}
	}

	private void Win()
	{
		SceneManager.LoadScene("You Win");
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;
public class MainMenu : MonoBehaviour 
{
	public AudioSource source;

	public void LoadScene(string scene)
	{
		source.Play();
		SceneManager.LoadScene(scene);
	}

	
	private void Update()
	{
		Player player = ReInput.players.GetPlayer(0);
		if(player.GetAnyButtonDown())
		{
			LoadScene("Main");
		}
	}
}

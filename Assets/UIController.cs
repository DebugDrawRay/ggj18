using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour 
{
	public static UIController instance;
	public Image[] hearts;

	public Image[] collectables;
	private void Awake()
	{
		instance = this;
	}

	public void UpdateHealth(int remaining)
	{
		for(int i = 0; i < hearts.Length; i++)
		{
			hearts[i].gameObject.SetActive(i < remaining);
		}
	}
	public void UpdateCollectables(int remaining)
	{
		for(int i = 0; i < collectables.Length; i++)
		{
			collectables[i].gameObject.SetActive(i < remaining);
		}
	}
}

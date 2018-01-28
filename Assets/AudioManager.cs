using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour 
{
	public static AudioManager instance;
	public float sfxVolume;
	public int initialSources = 3;
	private List<AudioSource> sources = new List<AudioSource>();

	private void Awake()
	{
		for(int i = 0; i < initialSources; i++)
		{
			AddSource();
		}
		instance = this;
	}

	private AudioSource AddSource()
	{
		AudioSource source = gameObject.AddComponent<AudioSource>();
		source.volume = sfxVolume;
		source.playOnAwake = false;
		source.loop = false;
		source.Stop();
		sources.Add(source);
		return source;
	}

	public void PlaySfx(AudioClip clip)
	{
		AudioSource source = FindAvailableSource();
		source.clip = clip;
		source.Play();
	}

	public AudioSource AddSourcePersistent()
	{
		return gameObject.AddComponent<AudioSource>();
	}
	private AudioSource FindAvailableSource()
	{
		foreach(AudioSource source in sources)
		{
			if(!source.isPlaying)
			{
				return source;
			}
		}
		return AddSource();
	}
	
}

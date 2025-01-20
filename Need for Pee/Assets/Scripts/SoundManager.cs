using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
	PEE,
	FLUSH,
	JINGLE,
	HATSUNE_MIKU,
	CVS,
	// VENDING_MACHINE,
	// FOOTSTEP,
	// BGM,
	// DOOR_OPEN,
	// BUTTON_CLICK,
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
	[SerializeField] private SoundList[] soundList;
	private static SoundManager instance;
	private AudioSource audioSource;

	private void Awake()
	{
		instance = this;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Start()
    {
#if !UNITY_EDITOR
		audioSource = GetComponent<AudioSource>();
		// play BGM
		audioSource.Play();
#endif
	}

    // Update is called once per frame
    private void Update()
    {
        
    }

	public static void PlaySound(SoundType type, float volume = 1)
	{
		Debug.Log("PlaySound: " + (int)type);
		// Randomly pick a sound in the soundList
		AudioClip[] clips = instance.soundList[(int)type].sounds;
        instance.audioSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)], volume);
	}

#if UNITY_EDITOR
    private void OnEnable()
    {
	    string[] names = Enum.GetNames(typeof(SoundType));
		Array.Resize(ref soundList, names.Length);
		for (int i = 0; i < soundList.Length; i++)
		{
		    soundList[i].name = names[i];
		}
	}
#endif
}

[Serializable]
public struct SoundList
{
	[HideInInspector] public string name;
	[SerializeField] public AudioClip[] sounds;
}

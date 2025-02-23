using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using KinematicCharacterController.Examples;

public enum SoundType
{
	PEE,
	SHORT_PEE,
	FLUSH,
	JINGLE,
	HATSUNE_MIKU,
	CVS,
	DOOR_OPEN,
	DOOR_CLOSE,
	ITEM,
	SPRINT,
	// VENDING_MACHINE,
	// BUTTON_CLICK,
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
	[SerializeField] private SoundList[] soundList;
	private static SoundManager instance;
	private AudioSource bgmAudioSource;
	private AudioSource otherAudioSource;
	public AudioSource footstepsSound;
	public ExamplePlayer player;

	private void Awake()
	{
		Debug.Log("soundmanager awake");
		instance = this;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Start()
    {
		player = FindAnyObjectByType<ExamplePlayer>();
		bgmAudioSource = GetComponents<AudioSource>()[0];
		otherAudioSource = GetComponents<AudioSource>()[1];
#if UNITY_EDITOR
	if (EditorApplication.isPlaying)
	{
		// play BGM
		bgmAudioSource.Play();
	}
#else
		// play BGM
		bgmAudioSource.Play();
#endif
	}

    // Update is called once per frame
    private void Update()
    {
		if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && !player.GetDisabled())
		{
			footstepsSound.enabled = true;
		}
		else
		{
			footstepsSound.enabled = false;
		}
	}

	public static void PlaySound(SoundType type, float volume = 1)
	{
		Debug.Log("PlaySound: " + (int)type);
		// Randomly pick a sound in the soundList
		SoundList soundList = instance.soundList[(int)type];
		AudioClip[] clips = soundList.sounds;
		instance.otherAudioSource.clip = clips[UnityEngine.Random.Range(0, clips.Length)];
		instance.otherAudioSource.volume = volume;
		instance.otherAudioSource.Play();
	}

	public static void StopSound()
	{
		instance.otherAudioSource.Stop();
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

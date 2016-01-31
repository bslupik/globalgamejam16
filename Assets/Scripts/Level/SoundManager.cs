using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] sounds;
    public AudioSource soundSource;
    public FadingMusic backgroundMusic;

	public void Start()
	{
    }
	
	public void Update()
	{
	}

    public void PlayBackground(int targetClip)
    {
        backgroundMusic.Play(targetClip);
    }
    
    public void PlaySound(int soundIndex)
    {
        soundSource.PlayOneShot(sounds[soundIndex]);
    }

    public void PlayActionSound(int actionIndex, int scoreIndex)
    {
        PlaySound(actionIndex);
        Callback.FireAndForget(() => PlaySound(scoreIndex), sounds[actionIndex].length, this);
    }
}

using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] sounds;
    public AudioSource soundSource;
    public AudioSource backgroundMusic;
    public FadingMusic foregroundMusic;

	public void Start()
	{
        foregroundMusic.Start();
    }
	
	public void Update()
	{
	}

    public void PlayForeground(int targetClip)
    {
        foregroundMusic.Play(targetClip);
    }


    public void PlaySound(int soundIndex)
    {
        soundSource.PlayOneShot(sounds[soundIndex]);
    }
}

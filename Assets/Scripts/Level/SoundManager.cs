using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] sounds;
    public AudioSource soundSource;

	public void Start()
	{
        soundSource = GetComponent<AudioSource>();
    }
	
	public void Update()
	{
	    
	}

    public void PlaySound(int soundIndex)
    {
        soundSource.PlayOneShot(sounds[soundIndex]);
    }
}

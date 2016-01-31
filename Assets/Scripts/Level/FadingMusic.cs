using UnityEngine;
using System.Collections;

public class FadingMusic : MonoBehaviour {

    enum FadeState { None, Playing, FadingIn, FadingOut };

    public const float WEIGHT = 0.5f;

    public AudioClip[] musicClips;
    public AudioSource music;
    private int targetClip;
    private FadeState state;

	public void Start () {
        if(musicClips.Length >= 5)
        {
            music.clip = musicClips[4];
            music.Play();
            state = FadeState.FadingIn;
        }
        else
        {
            state = FadeState.None;
        }
        music.volume = 0.1f;
	}
	
	public void Update () {
        if(state == FadeState.FadingOut)
        {
            if(music.volume <= 0.1f)
            {
                if(targetClip != -1)
                {
                    music.clip = musicClips[targetClip];
                    music.Play();
                    state = FadeState.FadingIn;
                    targetClip = -1;
                }
                else
                {
                    music.Stop();
                    music.clip = null;
                    state = FadeState.None;
                }
            }
            else
            {
                music.volume -= WEIGHT * Time.deltaTime;
            }
        }
        else if(state == FadeState.FadingIn)
        {
            if (music.volume >= 1)
            {
                music.volume = 1;
                state = FadeState.Playing;
            }
            else
            {
                music.volume += WEIGHT * Time.deltaTime;
            }
        }
	}

    public void Play(int targetClip)
    {
        if(targetClip >= 0 && targetClip < musicClips.Length)
        {
            this.state = FadeState.FadingOut;
            this.targetClip = targetClip;
        }
        else
        {
            this.state = FadeState.None;
            this.targetClip = -1;
            music.Stop();
            music.clip = null;
        }
    }
}

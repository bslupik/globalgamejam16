using UnityEngine;
using System.Collections;

public class SceneChangeScripting : MonoBehaviour {

    Blit[] blits;

	// Use this for initialization
	void Start () {
        blits = GetComponents<Blit>();

        //for testing
        //Callback.FireAndForget(FadeOut, 10, this);
	}

    public Coroutine FadeOut()
    {
        return Callback.DoLerp((float l) => { foreach (Blit blit in blits) blit.fadeProgress = l; }, 2, this);
    }

    public Coroutine FadeIn()
    {
        return Callback.DoLerp((float l) => { foreach (Blit blit in blits) blit.fadeProgress = l; }, 2, this, reverse: true, mode: Callback.Mode.REALTIME);
    }
}

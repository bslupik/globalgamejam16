using UnityEngine;
using System.Collections;

public class SceneChangeScripting : MonoBehaviour {

    Blit blit;

	// Use this for initialization
	void Start () {
        blit = GetComponent<Blit>();

        //for testing
        //Callback.FireAndForget(FadeOut, 10, this);
	}

    public Coroutine FadeOut()
    {
        return Callback.DoLerp((float l) => blit.fadeProgress = l, 2, this);
    }

    public Coroutine FadeIn()
    {
        return Callback.DoLerp((float l) => blit.fadeProgress = l, 2, this, reverse : true, mode : Callback.Mode.REALTIME);
    }
}

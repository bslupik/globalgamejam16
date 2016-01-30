using UnityEngine;
using System.Collections;

public class SceneChangeScripting : MonoBehaviour {

    Blit blit;

	// Use this for initialization
	void Start () {
        blit = GetComponent<Blit>();

        //for testing
        Callback.FireAndForget(FadeOut, 10, this);
	}

    public void FadeOut()
    {
        Callback.DoLerp((float l) => blit.fadeProgress = l, 2, this);
    }
}

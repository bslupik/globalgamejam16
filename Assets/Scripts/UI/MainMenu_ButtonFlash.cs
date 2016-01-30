using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu_ButtonFlash : MonoBehaviour {

    public Button target;
	
	// Update is called once per frame
	void Update ()
    {
        float scale = Mathf.PingPong(Time.time * 80, 50) + 120;
        target.image.rectTransform.sizeDelta = new Vector2(scale, scale);
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu_ButtonFlash : MonoBehaviour {

    public Button target;
	
	// Update is called once per frame
	void Update ()
    {
        Color temp = target.image.color;
        temp.a = Mathf.PingPong(Time.time * 80, 1);

        float scale = Mathf.PingPong(Time.time * 80, 50) + 120;
        target.image.rectTransform.sizeDelta = new Vector2(scale, scale);
	}
}

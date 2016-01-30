using UnityEngine;
using System.Collections;

public class OnBeatDraggable : Draggable {

	public override void Start ()
    {
		base.Start();
	}
	
	public override void Update ()
    {
		base.Update();
	}

    protected override void OnMouseDown()
    {
        if(base.level.OnBeat())
        {
            base.level.PlayerActed();
            StartCoroutine(base.DoDrag());
        }
        else
        {
            level.ScreenShake();
            // TODO: Play error sound
            // base.sound.PlaySound(0);
        }
    }

    void OnMouseUp()
    {
        base.level.PlayerActed();
    }
}

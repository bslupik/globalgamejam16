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
            StartCoroutine(DoDrag());
        }
        else
        {
            //level.ScreenShake();
            base.sound.PlaySound(0);
        }
    }

    protected virtual void OnMouseUp()
    {
        base.level.PlayerActed();
    }
}

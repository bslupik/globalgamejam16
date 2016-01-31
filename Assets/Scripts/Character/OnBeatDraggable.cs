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

    protected virtual void OnMouseUp()
    {
        base.level.PlayerActed();
    }
}

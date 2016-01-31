using UnityEngine;
using System.Collections;

public class PlayerNodeWalker : NodeWalker {

    private bool playerMoved;

	public override void Start () {
		base.Start();
        playerMoved = false;
	}
	
	public override void Update () {
		base.Update();

        if (!IsDone() && IsAtNode())
        {
            direction = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    public void PlayerMoved()
    {
        playerMoved = true;
    }

    protected override bool shouldMove()
    {
        if (base.shouldMove()) {
            if(playerMoved)
            {
                playerMoved = false;
                return true;
            }
        }
        return false;
    }
}

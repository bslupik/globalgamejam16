using UnityEngine;
using System.Collections;

public class Ghost : TimedLife
{
	public override void Start()
	{
		base.Start();
	}
	
	public override void Update()
	{
		base.Update();
	}

    public void OnMouseDown()
    {
        level.PlayerActed();
        GameObject.Destroy(gameObject);
    }

    public override void OnDeath()
    {
        level.GhostEscaped();
        base.OnDeath();
    }
}

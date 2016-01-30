using UnityEngine;
using System.Collections;

public class Ghost : TimedLife
{
    public float health;
    public float maxHealth;
    public float clickDamage;

	public override void Start()
	{
		base.Start();
        health = maxHealth;
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

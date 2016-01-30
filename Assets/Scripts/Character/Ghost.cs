using UnityEngine;
using System.Collections;

public class Ghost : TimedLife
{
    public float health;
    public float maxHealth = 100.0f;
    public float clickDamage = 100.0f;

	public override void Start()
	{
		base.Start();
        health = maxHealth;
    }
	
	public override void Update()
	{
		base.Update();
        if (health <= 0.0f)
        {
            OnDeath();
        }
	}

    public void OnMouseDown()
    {
        level.PlayerActed();
        health -= clickDamage;
    }

    public override void OnDeath()
    {
        if (health > 0)
        {
            level.GhostEscaped();
        }
        
        base.OnDeath();
    }
}

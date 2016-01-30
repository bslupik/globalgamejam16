using UnityEngine;
using System.Collections;

public class Villager : AbstractSwipable
{
    public float health;
    public float maxHealth = 100.0f;
    public float damagePerHit = 60.0f;
    public float minDamagedRatio = 0.6f;
    public bool damaged = false;
    
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
            KnockedOut();
        }
	}

    public override void Notify(SwipableMessage m)
    {
        health -= damagePerHit;
        if (health / maxHealth < minDamagedRatio)
        {
            SetToDamaged();
        }
    }

    public void SetToDamaged()
    {
        if (damaged)
        {
            return;
        }

        GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color / 2.0f;
        damaged = true;
    }

    public void KnockedOut()
    {
        GameObject.Destroy(gameObject);
    }
}

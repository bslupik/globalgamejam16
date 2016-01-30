using UnityEngine;
using System.Collections;

public class Villager : AbstractSwipable
{
    public float health;
    public float maxHealth = 100.0f;
    public float damagePerHit = 60.0f;
    public float minDamagedRatio = 0.6f;
    public GameObject target;
    public bool damaged = false;
    public float speed;
    public Vector3 direction;
    
	public override void Start()
	{
		base.Start();
        health = maxHealth;
        target = GameObject.FindWithTag("AttackTarget");
        Vector3 position = transform.position;
        Vector3 targetPosition = target.transform.position;
        direction = targetPosition - position;
        direction = Vector3.Normalize(direction);
    }
	
	public override void Update()
	{
		base.Update();
        if (health <= 0.0f)
        {
            KnockedOut();
        }
        
        transform.Translate(direction * speed);
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "AttackTarget")
        {
            level.EnemyGotThrough();
            GameObject.Destroy(gameObject);
        }
    }
}

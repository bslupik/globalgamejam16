using UnityEngine;
using System.Collections;

public class TimedLife : Base
{
    public float lifeTime;

	public override void Start()
	{
		base.Start();
	}
	
	public override void Update()
	{
		base.Update();
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0.0f)
        {
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        GameObject.Destroy(gameObject);
    }
}

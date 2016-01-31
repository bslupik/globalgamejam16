using UnityEngine;
using System.Collections;

public class TimedLife : Base
{
    public float lifeTime;
    public float lifeTimeBase;
    public float lifeTimeVariation;

	public override void Start()
	{
		base.Start();
        lifeTime = lifeTimeBase + Random.Range(-lifeTimeVariation, lifeTimeVariation);
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

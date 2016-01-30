using UnityEngine;
using System.Collections;

public class Arrow : TimedLife {

	public override void Start () {
		base.Start();
	}
	
	public override void Update () {
		base.Update();
	}

    public override void OnDeath()
    {
        level.ArrowMissed();
        base.OnDeath();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLLISION");
        Dodo dodo = collision.transform.GetComponent<Dodo>();
        if(dodo != null)
        {
            base.level.DodoKilled();
            GameObject.Destroy(dodo);
        }
    }
}

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
        base.OnDeath();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        RectRoamer dodo = collision.transform.GetComponent<RectRoamer>();
        if(dodo != null)
        {
            base.level.DodoKilled();
            GameObject.Destroy(dodo.gameObject);
            GameObject.Destroy(this.gameObject);
        }
    }
}

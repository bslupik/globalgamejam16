using UnityEngine;
using System.Collections;

public class Drift : Base
{
    public Vector3 direction;
    public float speed;

	public override void Start()
	{
		base.Start();
	}
	
	public override void Update()
	{
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
	}
}

using UnityEngine;
using System.Collections;
using System.IO;

public class Savable : Base
{
    public int savableID;
    public float timeToSpawn = 0.0f;

	public override void Start()
	{
		base.Start();
        GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>().RegisterSavable(this);
	}
	
	public override void Update()
	{
		base.Update();
	}

    public virtual void Save(StreamWriter output)
    {
        output.Write(timeToSpawn);
        output.Write(' ');
        output.Write(savableID);
        output.Write(' ');
        output.Write(transform.position.x);
        output.Write(' ');
        output.Write(transform.position.y);
        output.Write('\n');
    }
}

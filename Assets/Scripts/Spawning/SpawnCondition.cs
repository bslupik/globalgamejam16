using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnCondition : Base
{
    public GameObject spawnedObject;

    public override void Start()
	{
        base.Start();	
	}
	
	public override void Update()
	{
        base.Update();
	}

    public virtual bool ShouldSpawn()
    {
        return spawnedObject == null;
    }

    public virtual void OnSpawn(GameObject spawnObject)
    {
        spawnedObject = spawnObject;
    }
}
